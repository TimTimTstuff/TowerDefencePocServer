using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TStuff.Game.TowerDefense3d.lib.ContractObjects;
using TStuff.Game.TowerDefense3d.lib.Enums;
using TStuff.Game.TowerDefense3d.Server.Logic;
using TStuff.Game.TowerDefense3d.Server.Logic.AStar;
using TStuff.Game.TowerDefense3d.Server.Logic.Combat;

namespace TStuff.Game.TowerDefense3d.Server.Model.GameObjects
{
    public class ServerMob
    {
        private readonly GameSession _session;
      
        private readonly Tuple<MapTile, MapTile, MapTile> _wayPoints;
        public  MobData MobData { get; }
        private readonly double _waitBeforeDestroy = GConfig.D.MobWaitMillisecondsToDestroyBlockingTower;
        private  double WaitBeforeRemoved { get; set; } = GConfig.D.MobWaitMillisecondsBeforeUpdateRemoved;
        private double _destroyTimer = 0;
        private GameUser _sender;

        public bool VisitedWayPoint { get; set; }
        public int InstanceId { get; set; }
        public int MobId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public List<PathFinderNode> Path { get; set; }
        public int GoalX { get; set; }
        public int GoalY { get; set; }
        public float Speed { get; set; }
        public float TilePosX { get; set; }
        public float TilePosY { get; set; }
        public PathFinderNode CurrentGoal { get; set; }
        public int CurrentPathPos { get; set; }
        public double LastUpdate { get; set; }
        public bool Delete { get; set; }
        public int SenderId { get; set; }
        public int Hp { get; set; }
        public bool IsDead { get; set; }
        public int SenderTeamId { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsOnGoal { get; set; }


        public ServerMob(GameSession session, MobData mobId, Tuple<MapTile, MapTile, MapTile> spawn, int toTeam, GameUser user)
        {

            _sender = user;
            _session = session;
            _wayPoints = spawn;

            SenderId = user.User.Id;
           
            user.MobsSend++;
            MobData = mobId;
            StatusEffect = new List<MobStatusEffectModel>();
            TargetTeam = toTeam;
           
            VisitedWayPoint = _wayPoints.Item2 == null;
            InstanceId = InstanceHelper.GetNewInstanceId();

            X = spawn.Item1.X;
            Y = spawn.Item1.Y;
            
            MobId = mobId.MobId;
            Speed = (float) mobId.Speed;
            Hp = mobId.Hp;

            TilePosX = spawn.Item1.X;
            TilePosY = spawn.Item1.Y;

        

            LastUpdate = 0;
            UpdatePath();

        }

        public int TargetTeam { get; set; }
        public List<MobStatusEffectModel> StatusEffect { get; set; }

        public void Update(double delta)
        {
            if (IsDead || IsOnGoal)
            {
                WaitBeforeRemoved -= delta;
                if (WaitBeforeRemoved <= 0)
                {
                    RemoveMob();
                }
                return;
            }
            StatusEffect = StatusCalculator.GetCleardEffectList(StatusEffect);
            StatusCalculator.CalculateLeftTime(StatusEffect,delta);
            StatusCalculator.CalculateHot(MobData,StatusEffect,delta);
            StatusCalculator.CalculateDot(MobData,StatusEffect,delta, towerInstanceId =>
            {
                Killed(_session.GameTower.SingleOrDefault(x=>x.InstanceId == towerInstanceId));
            } );
            if (IsTowerCollision(delta)) return;
            var transformedSpeed = (float)StatusCalculator.GetSpeedValueByStatus(MobData,StatusEffect);
            
            if (CurrentGoal.X > TilePosX)
            {
                TilePosX += transformedSpeed / GConfig.D.GameSimulationTime * (float)delta;
            }
            if (CurrentGoal.X < TilePosX)
            {
                TilePosX -= transformedSpeed / GConfig.D.GameSimulationTime * (float)delta;
            }
            if (CurrentGoal.Y > TilePosY)
            {
                TilePosY += transformedSpeed / GConfig.D.GameSimulationTime * (float)delta;
            }
            if (CurrentGoal.Y < TilePosY)
            {
                TilePosY -= transformedSpeed / GConfig.D.GameSimulationTime * (float)delta;
            }
            if (!IsMobNearPoint(this)) return;
            
            X = CurrentGoal.X;
            Y = CurrentGoal.Y;
            CurrentPathPos++;
            if (Path == null) return;
            if (CurrentPathPos >= Path.Count)
            {
                if (VisitedWayPoint)
                {
                    ReachGoal();
                }
                else
                {
                    VisitedWayPoint = true;
                    UpdatePath();
                }
                return;
            }

            CurrentGoal = Path[CurrentPathPos];
        }

        private bool IsTowerCollision(double delta)
        {
            if (_session.MapHandler.IsTowerOnPosition(CurrentGoal.X, CurrentGoal.Y))
            {
                if (!UpdatePath())
                {
                    IsBlocked = true;
                    _destroyTimer += delta;
                    TStuffLog.Debug("I Wait");
                    if (_destroyTimer >= _waitBeforeDestroy)
                    {
                        _session.MapHandler.DestroyTower(this, CurrentGoal.X, CurrentGoal.Y);
                        _destroyTimer = 0;
                    }
                    return true;
                }
                _destroyTimer = 0;
                IsBlocked = false;
            }
            return false;
        }

        public bool UpdatePath()
        {
            var start = new Point(X, Y);
            var goal = VisitedWayPoint ? new Point(_wayPoints.Item3.X, _wayPoints.Item3.Y) : new Point(_wayPoints.Item2.X, _wayPoints.Item2.Y);
            Path = _session.MapHandler.GetPath(start,goal);
            GoalX = goal.X;
            GoalY = goal.Y;
            if (Path == null) return false;
            CurrentGoal = Path[0];
            CurrentPathPos = 0;
            return Path != null;
        }

        public void Hit(MobHit hit)
        {

            if (hit.Status != null)
            {
                if (StatusEffect.All(s => hit.Status.Type != s.Type))
                {
                    hit.Status.LeftDuration = hit.Status.Duration;
                    StatusEffect.Add(hit.Status);
                }
            }
            //Calculate Splash
            if (hit.SendingTower.TowerData.SplashRange > 0  &&
                !hit.NoSplash)
            {
                var mobsInDistance = GetMobsInDistance(this, hit.SendingTower.TowerData.SplashRange);
                var splashHit = new MobHit
                {
                    Status = hit.Status,
                    SendingTower = hit.SendingTower,
                    NoSplash = true
                };
                mobsInDistance.ForEach(m =>
                {
                    splashHit.Damage = hit.Damage;
                        
                    m.Hit(splashHit);
                });
            }
            
                Hp -= hit.Damage;
        

            
            if (Hp <= 0) { Killed(hit.SendingTower); return; }
        }

        private List<ServerMob> GetMobsInDistance(ServerMob serverMob, int towerDataSplashRange)
        {
            try
            {
                return _session.GameMobs.Where(m => MobInDistance(m, towerDataSplashRange)).ToList();
            }
            catch { return new List<ServerMob>();}
        }

        public bool TowerInDistance(ServerTowerModel tower, int distance)
        {
            return GetDistance(tower) <= distance;
        }

        public bool MobInDistance(ServerMob mob, int distance)
        {
            return GetDistance(mob) <= distance;
        }


        public double GetDistance(ServerMob mob)
        {
            return GetDistance(mob.X, mob.Y);
        }

        public double GetDistance(ServerTowerModel tower)
        {
            return GetDistance(tower.X, tower.Y);

        }

        public double GetDistance(int argx, int argy)
        {
            var dist = Math.Sqrt(Math.Pow(X - argx, 2) + Math.Pow(Y - argy, 2));
            if (dist < 0) dist = dist * -1;
            return dist;
        }


        private void ReachGoal()
        {
            if(_session.GameData.TeamHp[TargetTeam] > 0)
            _session.GameData.TeamHp[TargetTeam]--;

            IsOnGoal = true;
           
        }

      
        public void Killed(ServerTowerModel hitSendingTower)
        {
            IsDead = true;
            hitSendingTower?.KilledMob(MobData);
           
        }
 
        private void RemoveMob()
        {

            _session.GameMobs.Remove(this);
        }

        private static bool IsMobNearPoint(ServerMob m)
        {
            return ((int)Math.Floor(m.TilePosY + GConfig.D.TileDistanceDefaultTollerance) == m.CurrentGoal.Y) && ((int)Math.Floor(m.TilePosX + GConfig.D.TileDistanceDefaultTollerance) == m.CurrentGoal.X);
        }

        public MobMovementModel ToMobMoveObject()
        {
            LastUpdate = new TimeSpan(DateTime.UtcNow.Ticks).TotalMilliseconds;
            return new MobMovementModel
            {
                X = TilePosX,
                Y = TilePosY,
                Id = InstanceId,
                Hp = Hp,
                MobId = MobId,
                Speed = Speed,
                Status = StatusEffect.Select(x => x.Type).ToArray(),
                WorldStatus = IsDead || IsOnGoal ? (IsDead ? MobState.Dead : MobState.Goal) : MobState.Alive
            };
        }
    }
}
