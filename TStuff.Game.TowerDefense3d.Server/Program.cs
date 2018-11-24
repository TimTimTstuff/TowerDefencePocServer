using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.DPSBase;
using Newtonsoft.Json;
using TStuff.Game.TowerDefense3d.lib.ContractObjects;
using TStuff.Game.TowerDefense3d.lib.Enums;
using TStuff.Game.TowerDefense3d.Server.Managers;
using TStuff.Game.TowerDefense3d.Server.Model;
using TStuff.Game.TowerDefense3d.Server.Model.Map;
using TStuff.Game.TowerDefense3d.Server.Services;

namespace TStuff.Game.TowerDefense3d.Server
{
   public class GConfig
    {
        private static GameServerConfiguration _d;
        public static GameServerConfiguration D {
            get => _d ?? new GameServerConfiguration();
            set => _d = value;
        }
    }
    class Program
    {
        

        static void Main(string[] args)
        {
            TStuffLog.LogLevel = LogLevel.Info;
            TStuffLog.LogFileLevel = LogLevel.Info;
            TStuffLog.LogActions.Add((message, serializedObject, level, member, filepat, line) =>
            {
                if (TStuffLog.LogLevel > level) return;
                switch (level)
                {
                    case LogLevel.Trace:
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case LogLevel.Debug:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        break;
                    case LogLevel.Info:
                        
                        break;
                    case LogLevel.Warning:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case LogLevel.Error:
                    case LogLevel.Fatal:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(level), level, null);
                }
                Console.WriteLine("[{5},{0}/{1}:{2}, Level:{3}] Message: {4}",filepat.Split('\\').Last(),member,line,level.ToString(),message,DateTime.Now.ToString("T"));
                Console.ResetColor();
            });
            
            NetworkComms.DisableLogging();

            if (File.Exists("config.json"))
            {
                GConfig.D = JsonConvert.DeserializeObject<GameServerConfiguration>(File.ReadAllText("config.json"));
            }
            else
            {
                GConfig.D = new GameServerConfiguration();
                File.WriteAllText("config.json", JsonConvert.SerializeObject(GConfig.D, Formatting.Indented));
            }
            TStuffLog.Info("Server Started");
            TStuffLog.LogLevel = GConfig.D.ServerLogLevel;
            TStuffLog.LogFileLevel = GConfig.D.LogFileLevel;

            DataSerializer ds = DPSManager.GetDataSerializer<ProtobufSerializer>();
            NetworkComms.DefaultSendReceiveOptions = new SendReceiveOptions(ds, new List<DataProcessor>(), new Dictionary<string, string>());
            var gamecontext = SetupGameRegister();
            SetupGlobalNetworkHandler(gamecontext);

            GConfig.D.GameSimulationTime = 400;
            var cont = new LoginService(gamecontext);
            var map = new CreateGameService(gamecontext);


            Connection.StartListening(ConnectionType.TCP, new IPEndPoint(IPAddress.Any, GConfig.D.ServerPort));
            //Connectc fake user
            gamecontext.User.AddSystemUser(GConfig.D.MainBotName + " 0",0);
            gamecontext.User.AddSystemUser(GConfig.D.MainBotName + " 1",1);
            gamecontext.User.AddSystemUser(GConfig.D.MainBotName + " 2",2);
            gamecontext.User.AddSystemUser(GConfig.D.MainBotName + " 3",3);
            gamecontext.User.AddSystemUser(GConfig.D.MainBotName + " 4",4);


            //File.WriteAllText("mobs.json",JsonConvert.SerializeObject(gamecontext.Mobs));
            //File.WriteAllText("tower.json", JsonConvert.SerializeObject(gamecontext.Towers));
            //File.WriteAllText("race_mob.json", JsonConvert.SerializeObject(gamecontext.MobRaces));
            //File.WriteAllText("race_tower.json", JsonConvert.SerializeObject(gamecontext.TowerRaces));

            Console.ReadLine();
        }

        private static void SetupGlobalNetworkHandler(GlobalRegister gamecontext)
        {
            #region Global Handler
            
            NetworkComms.AppendGlobalConnectionEstablishHandler(connection => { TStuffLog.Debug("Icomming connection"); });
            NetworkComms.AppendGlobalConnectionCloseHandler(connection =>
            {
                gamecontext.GameSessions.LeaveGame(connection);
                gamecontext.User.RemoveUser(connection);
            });

            NetworkComms.AppendGlobalIncomingPacketHandler<EchoObject>("echo", (header, connection, incomingObject) =>
            {
                TStuffLog.Debug("Echo Object");
                TStuffLog.Debug($"Object info {incomingObject.Id} - {incomingObject.Message}");
                connection.SendObject("echo", incomingObject);
            });

            #endregion
        }

        private static GlobalRegister SetupGameRegister()
        {
            var gamecontext = new GlobalRegister();

            
            return gamecontext;
        }

     

       
    }
}
