using System;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using TStuff.Game.TowerDefense3d.lib;
using TStuff.Game.TowerDefense3d.lib.ContractObjects;
using TStuff.Game.TowerDefense3d.Server.Managers;

namespace TStuff.Game.TowerDefense3d.Server.Services
{
   public class LoginService
    {
        private readonly UserHandler _userHandler;
        private GlobalRegister _gr;
   

        public LoginService( GlobalRegister gr)
        {
            _userHandler = gr.User;
            _gr = gr;
          
            NetworkComms.AppendGlobalIncomingPacketHandler<Login>("login", (header, connection, incomingObject) =>
            {
                Process(connection,incomingObject);
            });
        }

        public void Process(Connection con, Login login )
        {
            TStuffLog.Debug("Get Login");

            if (_userHandler.Login(con, login))
            {
                TStuffLog.Info("Login for: " + login.Name + " Successfull");
                //Update Game data
                var user = _userHandler.GetUserByConnection(con);
                user.Send(RequestNames.GetObjectDb, new RaceModels
                {
                    TowerRaces = _gr.TowerRaces.ToArray(),
                    MobRaces = _gr.MobRaces.ToArray(),
                    Towers = _gr.Towers.ToArray(),
                    Mobs = _gr.Mobs.ToArray()
                });

            }
            else
            {
                TStuffLog.Warning("Login Error!");
            }
            
        }
    }
}
