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
 

   public class DemoBotImpl : GameBot
    {
      


        public override void Update(double delta)
        {
          

                var pMoney = _botUser.Money;

                var mId = _gr.Mobs.Where(m => m.Cost < pMoney).OrderByDescending(m => m.Cost).FirstOrDefault()?.MobId;
                if (mId == null) return;

                if (_session.RoundnUmber % 2 == 0)
                {
                    _session.Commands.Push(new GameCommands
                    {
                        Command = GameCommand.SendMob,
                        Id = (int)mId,
                        SenderId = _botUser.User.Id,
                        IntVal = 0
                    });
                 
                }
                else
                {
                
                    var tId = _gr.Towers.Where(m => m.GoldCost < pMoney).OrderByDescending(m => m.GoldCost)
                        .FirstOrDefault()?.TowerId;
                    var buildableTile = _session.MapHandler.GetRandomBuildTile(_teamId);
                    if (tId != null && buildableTile != null)
                    {
                        _session.Commands.Push(new GameCommands
                        {
                            X = buildableTile.X,
                            Y = buildableTile.Y,
                            Id = tId.Value,
                            SenderId = _botUser.User.Id,
                            Command = GameCommand.BuildTower
                        });
                    }
                }

            
        }

        public DemoBotImpl(GlobalRegister gr, GameSession session,GameUser user,int teamId) : base(gr, session,user,teamId)
        {
        }
    }
}
