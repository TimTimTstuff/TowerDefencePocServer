using System;
using System.Collections.Generic;
using System.Linq;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using TStuff.Game.TowerDefense3d.lib;
using TStuff.Game.TowerDefense3d.lib.ContractObjects;
using TStuff.Game.TowerDefense3d.lib.Enums;
using TStuff.Game.TowerDefense3d.Server.Logic;
using TStuff.Game.TowerDefense3d.Server.Logic.Bot;
using TStuff.Game.TowerDefense3d.Server.Managers;
using TStuff.Game.TowerDefense3d.Server.Model;
using TStuff.Game.TowerDefense3d.Server.Model.GameObjects;

namespace TStuff.Game.TowerDefense3d.Server.Services
{
   public class CreateGameService
    {
        private GlobalRegister _gr;

        public CreateGameService(GlobalRegister gamecontext)
        {
            _gr = gamecontext;
            NetworkComms.AppendGlobalIncomingPacketHandler<MapRequests>(RequestNames.GetMapInfos, (header, connection, incomingObject) =>
            {
                ProcessMapRequests(connection, incomingObject);
            });

            NetworkComms.AppendGlobalIncomingPacketHandler<GameSettings>(RequestNames.StartGame,(
                (header, connection, incomingObject) =>
                {
                    var user = _gr.User.GetUserByConnection(connection);
                    CreateGameWithSettings(user,incomingObject);
                } ));

            NetworkComms.AppendGlobalIncomingPacketHandler<JoinGame>(RequestNames.JoinGame, (header, connection, incomingObject) =>
            {
                var user = _gr.User.GetUserByConnection(connection);
                JoinOpenGame(user, incomingObject);
            });

            NetworkComms.AppendGlobalIncomingPacketHandler<GameNotifications>(RequestNames.GameNotification, (header, connection, incomingObject) =>
            {
                var user = _gr.User.GetUserByConnection(connection);
                ProcessGameNotifications(incomingObject, connection);
            });
            NetworkComms.AppendGlobalIncomingPacketHandler<GameCommands>(RequestNames.GameCommand,
                (header, connection, incomingObject) =>
                {
                   
                    var user = _gr.User.GetUserByConnection(connection);
                    var session = _gr.GameSessions.GetSessionByUserId(user);
                    incomingObject.SenderId = user.Id;   
                    session.Commands.Push(incomingObject);
                });

        }

        private void ProcessGameNotifications(GameNotifications incomingObject, Connection connection)
        {
            var user = _gr.User.GetUserByConnection(connection);
            var session = _gr.GameSessions.GetSessionByUserId(user);
            var player = session.Players.Single(p => p.User.Id == user.Id);
             switch (incomingObject.Notification)
            {
                case GameNotificationType.GameCanceled:
                case GameNotificationType.LeaveGame:
                    _gr.GameSessions.LeaveGame(connection);
                    return;
                case GameNotificationType.Ready:
                    session.Players.Single(p => p.User.Id == user.Id).State = incomingObject.IntValue == 1 ? PlayerState.Ready : PlayerState.Joined;
                    break;
                case GameNotificationType.ChangeTeam:
                  
                    session.Teams.Single(p => p.Value.Any(x => x.User.Id == user.Id)).Value.Remove(player);
                    session.Teams[incomingObject.IntValue].Add(player);
                    player.TeamId = incomingObject.IntValue;
                    break;
                case GameNotificationType.StartGame:
                    TStuffLog.Info("Start Game");
                    //BotAdd
                    StartupGame(session);
                    break;
                
            }

            var joinInfo = _gr.GameSessions.GetGameJoinInfo(session);
           session.Players.ForEach(p =>
           {
               p.User.Send(RequestNames.GameJoinInfo, joinInfo);
           });
        }

        private void StartupGame(GameSession session)
        {
           

            session.State = GameState.LoadState;
            session.Players.ForEach(p => p.State = PlayerState.Loading);
            try
            {
                var mapLoader = new MapFromImageParser(session.SelectedMap);
                session.MapData = mapLoader.ParseImage();

                session.MapHandler = new ServerMap(session);

                if (session.Players.Count == 1)
                {
                    AddRace(session, 1, 0,0);
                    //AddRace(session, 0, 1, 0);
                    //AddRace(session, 2, 2, 1);
                    //AddRace(session, 3, 3, 1);

                }

                session.GameLoop = new GameLoop(_gr, session);
            }
            catch (Exception ex)
            {
                TStuffLog.Error(ex.ToString());
            }
            session.Players.ForEach(p => p.User.Send(RequestNames.GetMapData, session.MapData));
           
        }

        private void AddRace(GameSession session, int teamId, int userId,int raceId)
        {
            var temp = session.Players.FirstOrDefault();
            var bot = new GameUser
            {
                User = _gr.User.GetSystemUser(userId),
                Money = temp.Money,
                Income = temp.Income,
                Kills = temp.Income,
                MobsSend = temp.MobsSend,
                TeamId = teamId,
                State = PlayerState.Loading
            };
            session.Bots.Add(new DemoBotImpl(_gr,session,bot,teamId));
        }

        private void AddBott(GameSession session, int teamId, int userId)
        {
            var temp = session.Players.FirstOrDefault();
            var bot = new GameUser
            {
                User = _gr.User.GetSystemUser(userId),
                Money = temp.Money,
                Income = temp.Income,
                Kills = temp.Income,
                MobsSend = temp.MobsSend,
                TeamId = teamId,
                State = PlayerState.Loading
            };
            session.Bots.Add(new DemoBotImpl(_gr, session, bot, teamId));
        }

        private void JoinOpenGame(User user, JoinGame incomingObject)
        {
            var session = _gr.GameSessions.Sessions.SingleOrDefault(g => g.Id == incomingObject.GameId);
     
         
            if (session == null)
            {
                user.Send(RequestNames.Message,new ClientMessage{MessageType = MessageType.Info,Title = "Cant find game",Message = "The selected Game cant be found!"});
                return;
            }
            if (session.Players.Count >= session.SelectedMap.Teams * session.SelectedMap.MaxPlayers)
            {
                user.Send(RequestNames.Message, new ClientMessage { MessageType = MessageType.Info, Title = "Game is full", Message = "There are no more places to join" });
                return;
            }

          
            _gr.GameSessions.PlayerJoinSession(session, user);
            
            
        }

        private void CreateGameWithSettings(User user, GameSettings incomingObject)
        {
            //If a user has allready a GameSession
            if (_gr.GameSessions.GetSessionByUserId(user) != null)
            {
                user.Send(RequestNames.Message,new ClientMessage{Message = "There exists a game which you have created!",MessageType = MessageType.Error,Title = "Create Game Error"});
                return;
            };

            _gr.GameSessions.CreateGameSession(user,incomingObject);
         TStuffLog.Info("Game Startet!");
            //Join Connection Player;
        }

        private void ProcessMapRequests(Connection connection, MapRequests incomingObject)
        {
            var user = _gr.User.GetUserByConnection(connection);
            TStuffLog.Debug($"Incomming Request with: {incomingObject.Request.ToString()}");
            //DemoMaps
            switch (incomingObject.Request)
            {
                case MapRequestTypes.GetAllMaps:
                    SendMapListToClient(user);
                    break;
                case MapRequestTypes.GetOpenGames:
                    SendOpenGames(user);
                    break;
                    
            }
        }

        private void SendOpenGames(User user)
        {
           var result = _gr.GameSessions.Sessions.Where(s => s.State == GameState.JoinState).Select(a => new OpenGameInfo
           {
               Owner = a.Players.First(b=>b.Owner).User.Name,
               MapName = _gr.MapListManager.GetMapInfoById(a.SelectedMap.Id),
               Players = a.Players.Count,
               GameId = a.Id
           }).ToArray();
            user.Send(RequestNames.GetOpenGames,result);
        }

        private void SendMapListToClient(User user)
        {
            user.Send(RequestNames.GetMapList,new MapSelectList
            {
                Maps = _gr.MapListManager.GetMapListNetwork()
            });
        }
    }


}
