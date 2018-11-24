using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TStuff.Game.TowerDefense3d.lib.ContractObjects;
using TStuff.Game.TowerDefense3d.Server.Managers;
using TStuff.Game.TowerDefense3d.Server.Model;

namespace TStuff.Game.TowerDefense3d.Server.Logic.Bot
{
   public class RaceTestBot : GameBot
   {
       private Stack<MapTile> _myMap = new Stack<MapTile>();
       private bool _isMobRound;
       private int _raceId;
       private List<TowerData> _myTower = new List<TowerData>();
       private Dictionary<int,int> _towersBuild = new Dictionary<int, int>();
       private int _maxPerUniqTower;

       public RaceTestBot(GlobalRegister gr, GameSession session, GameUser botPlayer, int teamId,int raceId) : base(gr, session, botPlayer, teamId)
       {
           _maxPerUniqTower = 200;
           _raceId = raceId;
            foreach (var mapTile in session.MapData.Data.Where(a => a.BuildableByTeamId == _teamId).OrderBy(d => d.X)
                .ThenByDescending(d => d.Y).ToList())
            {
                    _myMap.Push(mapTile);
            }

            _gr.TowerRaces[_raceId].Towers.Split(',').ToList().ForEach(a =>
            {
                _myTower.Add(_gr.Towers.Single(x => x.TowerId == int.Parse(a)));
                _towersBuild.Add(int.Parse(a),0);
            });
        }

        public override void Update(double delta)
        {
            if (_session.RoundnUmber % 2 == 0)
            {
                var mId = _gr.Mobs.Where(m => (m.Cost) < _botUser.Money).OrderByDescending(m => m.Cost).FirstOrDefault()?.MobId;
                if (mId == null) return;
                _session.Commands.Push(new GameCommands
                {
                    Command = GameCommand.SendMob,
                    Id = (int)mId,
                    SenderId = _botUser.User.Id,
                    IntVal = 0
                });
                _isMobRound = true;
            }
            else
            {
                if(_myMap.Count <= 0)return;
               
                
                var tId = _myTower.Where(m => (m.GoldCost*2) < _botUser.Money && _session.GameTower.Count(a=> a.TowerId == m.TowerId && a.Owner.User.Id == _botUser.User.Id) <= _maxPerUniqTower).OrderByDescending(m => m.GoldCost)
                    .FirstOrDefault()?.TowerId;
               
                if (tId != null )
                {
                    var buildableTile = _myMap.Pop();
                    _session.Commands.Push(new GameCommands
                    {
                        X = buildableTile.X,
                        Y = buildableTile.Y,
                        Id = tId.Value,
                        SenderId = _botUser.User.Id,
                        Command = GameCommand.BuildTower
                    });
                    _towersBuild[tId.Value]++;

                }
                _isMobRound = false;
            }
        }
    }
}
