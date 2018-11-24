using System;
using System.Collections.Generic;
using System.Linq;
using NetworkCommsDotNet.Connections;
using TStuff.Game.TowerDefense3d.lib;
using TStuff.Game.TowerDefense3d.lib.ContractObjects;
using TStuff.Game.TowerDefense3d.lib.Enums;
using TStuff.Game.TowerDefense3d.Server.Logic.Bot;
using TStuff.Game.TowerDefense3d.Server.Model;
using TStuff.Game.TowerDefense3d.Server.Model.GameObjects;

namespace TStuff.Game.TowerDefense3d.Server.Managers
{
   public class GameSessionManager
   {
       private GlobalRegister _gr;
       private int _gameId = 0;

       public GameSessionManager(GlobalRegister gamecontext)
       {
           _gr = gamecontext;
       }


       public List<GameSession> Sessions { get;  } = new List<GameSession>();

       private GameSession GetSessionByConnection(Connection connection)
       {
           return Sessions.FirstOrDefault(u=>_gr.User.GetUserByConnection(connection)!=null);
       }

       public void CreateGameSession(User initiatingUser, GameSettings settings)
       {
           var map = _gr.MapListManager.Maps.SingleOrDefault(m => m.Id == settings.MapId);
           if( map == null)throw new Exception($"Map with ID {settings.MapId} not found");
            //TODO Change Settings
           var gamedata = new GameStats();
            gamedata.TeamHp = new List<int>
            {
                -1,-1,-1,-1
            };
           for (var i = 0; i < map.Teams; i++)
           {
               gamedata.TeamHp[i] = settings.Hp;
           }
           var gameSession =new GameSession
           {
               Id = _gameId,
               GameData = gamedata,
               State = GameState.JoinState,
               Players = new List<GameUser> { new GameUser { User = initiatingUser,Owner=true,State = PlayerState.Joined,Money = settings.StartGold, Income = settings.StartIncome} },
               Setting = new GamePartiesSetting
               {
                   MapId = settings.MapId,
                   RoundSeconds = settings.IncomeTime,
                   Lives = settings.Hp,
                   MaxMobs = settings.MaxMobs,
                   MaxPlayers = map.MaxPlayers,
                   StartGoldRound = settings.StartIncome,
                   TimeToStart = settings.TimeBeforeMobs,
                   Teams = map.Teams,
                   MaxMobsPerTeam = settings.MaxMobsPerTeam,
                   StartGold = settings.StartGold,
                   MobSpawnDelay=settings.MobSpawnDelay,
                   SpawnOnAll = true
                  },
               SelectedMap = _gr.MapListManager.Maps.SingleOrDefault(x=>x.Id == settings.MapId),
               Teams = new Dictionary<int, List<GameUser>>()
           };

            Sessions.Add(gameSession);
           initiatingUser.CurrentSession = gameSession;
           for (int i = 0; i < gameSession.Setting.Teams; i++)
           {
              gameSession.Teams.Add(i,new List<GameUser>());
           }
           gameSession.Teams[0].Add(gameSession.Players[0]);

           initiatingUser.Send(RequestNames.GameJoinInfo,GetGameJoinInfo(gameSession));
       }

   
        public void LeaveGame( Connection connection)
       {
           var session = GetSessionByConnection(connection);
           var user = _gr.User.GetUserByConnection(connection);
           if (session != null)
           {
               var player = (session?.Players).SingleOrDefault(a => a.User.Id == user.Id);
               if (player == null) return;
               if (player.Owner)
               {
                   CancelGame(session);
               }
               else
               {
                   session.Players.Remove(player);
                   foreach (var team in session.Teams)
                   {
                       team.Value.Remove(player);
                   }
                   try
                   {
                       user.Send(RequestNames.GameNotification,
                           new GameNotifications {Notification = GameNotificationType.GameCanceled});
                    }
                    catch(Exception ex) { TStuffLog.Error("Notification GameCancle "+ex);}
                   session.Players.ForEach(p =>
                   {
                       p.User.Send(RequestNames.GameJoinInfo, GetGameJoinInfo(session));
                   });
               }
           }
       }

       private void CancelGame(GameSession session)
       {
           session.Players.ForEach(p =>
           {
               try
               {
                   p.User.CurrentSession = null;
                   p.User.Send(RequestNames.GameNotification,
                       new GameNotifications {Notification = GameNotificationType.GameCanceled});

               }
               catch (Exception ex)
               {
                   TStuffLog.Error(ex.ToString());
               }
           });

            session.Players.Clear();
            session.GameMobs.Clear();
            session.GameTower.Clear();
            session.Dispose();
           Sessions.Remove(session);
       }

       public JoinGame GetGameJoinInfo(GameSession gameSession)
       {
           var info = new JoinGame
           {
               MapInfo = _gr.MapListManager.GetMapInfoById(gameSession.SelectedMap.Id),
               GameId = gameSession.Id,
               GameState = gameSession.State,
               
               PlayerInfo = gameSession.Players.Select(p=>new Player
               {
                   Name = p.User.Name,
                   Id = p.User.Id,
                   State = p.State,
                   Team = p.TeamId
               }).ToArray()
           };

           return info;
       }

       public void PlayerJoinSession(GameSession session, User user)
       {
           
           var player = new GameUser {User = user, State = PlayerState.Joined, Owner = false, Money = session.Setting.StartGold,Income = session.Setting.StartGoldRound};
            session.Players.Add(player);

           foreach (var team in session.Teams)
           {
               if (team.Value.Count >= session.SelectedMap.MaxPlayers) continue;
               team.Value.Add(player);
               player.TeamId = team.Key;
               break;
           }
           session.Players.ForEach(p =>
           {
               p.User.Send(RequestNames.GameJoinInfo,GetGameJoinInfo(session));
           });
           
       }

       public GameSession GetSessionByUserId(User user)
       {
           return Sessions.FirstOrDefault(u => u.Players.Any(us => us.User.Id == user.Id));
       }
   }
}
