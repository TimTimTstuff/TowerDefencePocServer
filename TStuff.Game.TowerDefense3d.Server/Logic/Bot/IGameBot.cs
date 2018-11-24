using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TStuff.Game.TowerDefense3d.Server.Managers;
using TStuff.Game.TowerDefense3d.Server.Model;

namespace TStuff.Game.TowerDefense3d.Server.Logic.Bot
{
    public abstract class GameBot : IGameBot
    {
        protected GameSession _session;
        protected GlobalRegister _gr;
        protected GameUser _botUser;
        protected int _teamId;

        protected GameBot(GlobalRegister gr, GameSession session, GameUser botPlayer,int teamId)
        {
            _gr = gr;
            _session = session;
            _botUser = botPlayer;
            _teamId = teamId;
            RegisterBot();
        }

        private void RegisterBot()
        {
            _session.Players.Add(_botUser);
            _session.Teams[_teamId].Add(_botUser);
            _botUser.TeamId = _teamId;
        }


        public abstract void Update(double delta);
    }


   public interface IGameBot
   {
       void Update(double delta);
   }
}
