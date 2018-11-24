using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using TStuff.Game.TowerDefense3d.lib;
using TStuff.Game.TowerDefense3d.lib.ContractObjects;
using TStuff.Game.TowerDefense3d.lib.Enums;
using TStuff.Game.TowerDefense3d.Server.Logic.AStar;
using TStuff.Game.TowerDefense3d.Server.Managers;
using TStuff.Game.TowerDefense3d.Server.Model;
using TStuff.Game.TowerDefense3d.Server.Model.GameObjects;

namespace TStuff.Game.TowerDefense3d.Server.Logic
{
    public class GameLoop
    {
        private readonly Timer _gameTimer;
        private readonly Timer _sendUpdateTimer;
        private readonly GameSession _session;
        private readonly Stack<FastUpdate> _fastUpdateObjectQueue = new Stack<FastUpdate>();
        private Stack<MapTowers> _mapTowerBuild = new Stack<MapTowers>();
        private readonly GlobalRegister _gr;
        private Random _random;
        
     
        #region Flags
        public bool _mapHasChanged;
        private readonly int _maxMobsSendUpdate = GConfig.D.MaxMobSendUpdate;
        private bool _allowSendEnemys = false;
        private bool _sendListReady = false;
        #endregion 

        #region Timer
        private double _startTime;
        private double _lastTime;
        private double _lastAliveMsg;
        private double _sendStatsTimer;
        private double _lastSendTime;
        private readonly double _startSendTime;

        private double _incomeTimer;
        private double _startEnemyTimer;
        private Dictionary<int,double> _playerMobSpawnDelay = new Dictionary<int, double>();
        

        #endregion

        public GameLoop(GlobalRegister gr, GameSession session)
        {
            TStuffLog.Info("Start Gameloop");
             _gr = gr;
            _session = session;
            InitializeGameLoopData();
           
         

            _startTime = new TimeSpan(DateTime.UtcNow.Ticks).TotalMilliseconds;
            _lastTime = _startTime;
            _gameTimer = new Timer(state =>
            {
                var delta = new TimeSpan(DateTime.UtcNow.Ticks).TotalMilliseconds - _lastTime;
                var total = new TimeSpan(DateTime.UtcNow.Ticks).TotalMilliseconds - _startTime;
                MaintainGameSession(delta);
                ProcessGame(delta, total);
                _lastTime = new TimeSpan(DateTime.UtcNow.Ticks).TotalMilliseconds;
            }, null, 0,  GConfig.D.LogicUpdateEachMillisecond);
            _startSendTime = new TimeSpan(DateTime.UtcNow.Ticks).TotalMilliseconds;
            _lastSendTime = _startTime;
            _sendUpdateTimer = new Timer(state =>
           {
               var delta = new TimeSpan(DateTime.UtcNow.Ticks).TotalMilliseconds - _lastSendTime;
               var total = new TimeSpan(DateTime.UtcNow.Ticks).TotalMilliseconds - _startSendTime;
               try
               {
                   SendDataState(delta);
               }
               catch (Exception ex)
               {
                   TStuffLog.Error(ex.ToString());
               }
               _lastSendTime = new TimeSpan(DateTime.UtcNow.Ticks).TotalMilliseconds;
           }, null, 0, GConfig.D.SendLoopMilliseconds);
         
        }


        #region Server Jobs
        private void InitializeGameLoopData()
        {
            _session.GameMobs = new List<ServerMob>();
            _session.GameTower = new List<ServerTowerModel>();
            _mapTowerBuild = new Stack<MapTowers>();
            _session.Commands = new Stack<GameCommands>();
            _random = new Random();

        }
        private void MaintainGameSession(double delta)
        {
            _lastAliveMsg += delta;
            if (_lastAliveMsg >= GConfig.D.GameSessionLoopAliveMessageAfterSeconds* GConfig.D.GameSimulationTime)
            {
               
                TStuffLog.Info("Game loop of Session: "+_session.Id+" is alive!");
                _lastAliveMsg = 0;
            }
        }
        private void ProcessGame(double delta, double total)
        {
            switch (_session.State)
            {
                case GameState.LoadState:
                    DoLoadStep(delta);
                    break;
                case GameState.PlayState:
                    try
                    {
                        PreUpdate(delta);
                        Update(delta);
                        PostUpdate(delta);
                    }
                    catch (Exception ex)
                    {
                        TStuffLog.Error(ex.ToString());
                    }
                    break;
                case GameState.FinishState:
                    break;
            }
        }
        private void DoLoadStep(double delta)
        {
            var allLoaded = _session.Players.Any(p => p.State == PlayerState.Loading);
            if (allLoaded) { _session.State = GameState.PlayState; }
            _startTime = new TimeSpan(DateTime.UtcNow.Ticks).TotalMilliseconds;
            _lastTime = _startTime;
            _session.Players.ForEach(p =>
            {
                _playerMobSpawnDelay.Add(p.User.Id,0);
            });
        }
        #endregion

        #region Update Methods

        private void PostUpdate(double delta)
        {

            _session.GameMobs.Remove(null);
            _session.GameTower.Remove(null);

            if (_fastUpdateObjectQueue.Count > 0) return;
            var mobs = _session.GameMobs.Where(a => a != null).OrderBy(r => r.LastUpdate).Take(_maxMobsSendUpdate).ToList();
            var tower = _session.GameTower.Where(a => a != null && a.hasChanged).OrderBy(r => r.LastUpdate).Take(_maxMobsSendUpdate)
                .ToList();

            var up = new FastUpdate
            {
                Mobs = mobs.Select(a => a.ToMobMoveObject()).ToArray(),
                Tower = tower.Select(a => a.ToTowerStateModel()).ToArray()
            };
            _fastUpdateObjectQueue.Push(up);


        }

        private void Update(double delta)
        {
            //Update Tower
            for (var index = 0; index < _session.GameTower.Count; index++)
            {
                var serverTowerModel = _session.GameTower[index];
                serverTowerModel?.Update(delta);
            }
            //Update Mobs
            for (var index = 0; index < _session.GameMobs.Count; index++)
            {
                ServerMob t = _session.GameMobs[index];
                t?.Update(delta);
            }

            //Do Bots
            _session.Bots.ForEach(b =>
            {
                b.Update(delta);
            });

        }

        private void PreUpdate(double delta)
        {
            _session.Players.ForEach(p =>
            {
                _playerMobSpawnDelay[p.User.Id] += delta;
            });
            _startEnemyTimer += delta;
            _incomeTimer += delta;

            //Enemy send Timer;
            if (_allowSendEnemys==false && _startEnemyTimer >= _session.Setting.TimeToStart * GConfig.D.GameSimulationTime)
            {
          
                _allowSendEnemys = true;

            }

            //Generate Income
            if (_incomeTimer >= _session.Setting.RoundSeconds * GConfig.D.GameSimulationTime)
            {
               
                GenerateIncome();
                _incomeTimer = 0;
            }

            var commandCount = 0;
            while (commandCount <= GConfig.D.ProcessNetCommandsPerUpdate && _session.Commands.Count > 0)
            {
                var c = _session.Commands.Pop();
                var user = _session.Players.SingleOrDefault(p => p.User.Id == c.SenderId);
                if (user == null) return;
                switch (c.Command)
                {
                    case GameCommand.SendMob:

                        if (!SpawnMob(delta,_gr.Mobs.Single(m=>m.MobId == c.Id),user))
                        {
                           // _session.Commands.Push(c);
                        }
                        break;
                    case GameCommand.BuildTower:
                        
                        if (!SpawnTower(delta, _gr.Towers.Single(t => t.TowerId == c.Id), user, c.X,c.Y))
                        {
                            
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                commandCount++;
            }
           
        }

        private void GenerateIncome()
        {
           _session.Players.ForEach(p =>
           {
               p.Money += p.Income;
               //TODO Send income message
           });
            _session.RoundnUmber++;
        }

        private void SendDataState(double delta)
        {
           
            SendUpdate();
            UpdateAndSendStats(delta);
        }
        #endregion


        #region Game Logic
        private bool SpawnTower(double delta, TowerData tower, GameUser user, int cX, int cY)
        { 
            if (tower.GoldCost > user.Money) return false;
            if (!_session.MapHandler.CanPlaceTower(user, cX, cY)) return false;

            user.Money -= tower.GoldCost;
            var newTower = new ServerTowerModel(user,tower,cX,cY,_session);
            _session.MapHandler.PlaceTower(newTower);
            _session.GameTower.Add(newTower);
            var sendObject = newTower.GetTowerInfo();
            _session.Players.ForEach(p =>
            {
                p.User.Send(RequestNames.TowerBuild,sendObject);
            });
            return true;
        }


        private bool SpawnMob(double delta, MobData mobId,  GameUser user)
        {
            //return false;
            if (!_allowSendEnemys) return false;
            if (!_session.MapHandler.IsReady) return false;
            if (_session.GameMobs.Count > _session.Setting.MaxMobs) return false;
            if (_playerMobSpawnDelay[user.User.Id] < _session.Setting.MobSpawnDelay) return false;
            if (user.Money < mobId.Cost) return false;
            if (_session.GameMobs.Count(m => m.SenderTeamId == user.TeamId) > _session.Setting.MaxMobsPerTeam) return false;

            user.Money -= mobId.Cost;

            user.Income += mobId.AddIncome;

            //Reset Values
            _playerMobSpawnDelay[user.User.Id] = 0;
            _session.Teams.Where(t=>t.Value.Count>0 && t.Key != user.TeamId).ToList().ForEach(t =>
            {
                 if (_session.Setting.SpawnOnAll)
            {
                _session.MapHandler.GetAllSpawns(t.Key).ForEach(m =>
                {
                    var mob = new ServerMob(_session, mobId, m, t.Key,user);
                    _session.GameMobs.Add(mob);
                   
                });
            }
            else
            {
                var mob = new ServerMob(_session, mobId, _session.MapHandler.GetRandomSpawn(t.Key), t.Key,user);
                _session.GameMobs.Add(mob);
            }



            });
           
           
            return true;
        }


        #endregion

        #region Game Send Data
        private void UpdateAndSendStats(double delta)
        {
            _sendStatsTimer += delta;
            if (_sendStatsTimer >= GConfig.D.SendStatsEachMilliseconds)
            {
                _sendStatsTimer = 0;
            }
            else
            {
                return;
            }
            var stats = new GameInfoData
            {
                IncomeTimer = (int) Math.Floor((_session.Setting.RoundSeconds)-(_incomeTimer/ GConfig.D.GameSimulationTime)),
                MobsOnWorld = _session.GameMobs.Count,
                HPTeam1 = _session.GameData.TeamHp[0],
                HPTeam2 = _session.GameData.TeamHp[1],
                HPTeam3 = _session.GameData.TeamHp[2],
                HPTeam4 = _session.GameData.TeamHp[3],
                AllIncome = _session.Players.Select(x=>x.User.Id+","+x.TeamId+","+x.Income).ToArray(),
                HighestIncome = _session.Teams.OrderByDescending(x=>_session.Teams[x.Key].Sum(v=>v.Income)).First().Key,
                
            };
            _session.Players.ForEach(p =>
            {
              
              
                stats.TeamIncomeTotal = _session.Teams[p.TeamId].Sum(x=>x.Income);
                stats.OwneIncome = p.Income;
                stats.OwneMoney = p.Money;
                p.User.Send(RequestNames.GameDataInfo,stats);
            });
        }
        private void SendUpdate()
        {
            if (_fastUpdateObjectQueue.Count == 0) return;
            var fu = _fastUpdateObjectQueue.Pop();
            if (fu == null) return;

            _session.Players.ForEach(p =>
            {
                try
                {
                    p.User.Send(RequestNames.FastUpdate, fu);
                }
                catch(Exception ex) {TStuffLog.Error(ex.StackTrace); }
            });
        }
        #endregion

        #region Helper

        public void StopLoop()
        {
            _gameTimer.Dispose();
            _sendUpdateTimer.Dispose();
        }

        #endregion

    }
}
