using System;
using System.Collections.Generic;
using TStuff.Game.TowerDefense3d.lib.ContractObjects;
using TStuff.Game.TowerDefense3d.lib.Enums;
using TStuff.Game.TowerDefense3d.Server.Logic;
using TStuff.Game.TowerDefense3d.Server.Logic.Bot;
using TStuff.Game.TowerDefense3d.Server.Model.GameObjects;
using TStuff.Game.TowerDefense3d.Server.Model.Map;

namespace TStuff.Game.TowerDefense3d.Server.Model
{

   
  public  class GameSession : IDisposable
    {
        public GameState State { get; set; }
        public MapModel SelectedMap { get; set; }
        public List<GameUser> Players { get; set; } = new List<GameUser>();
        public Dictionary<int, List<GameUser>> Teams { get; set; } = new Dictionary<int, List<GameUser>>();
        public GamePartiesSetting Setting { get; set; }
        public int Id { get; set; }
        public MapDataModel MapData { get; set; }
        public GameLoop GameLoop { get; set; }
        public List<ServerTowerModel> GameTower { get; set; } = new List<ServerTowerModel>();
        public List<ServerMob> GameMobs { get; set; } = new List<ServerMob>();
        public Stack<GameCommands> Commands { get; set; } = new Stack<GameCommands>();
        public GameStats GameData { get; set; }
        public ServerMap MapHandler { get; set; }
        public int RoundnUmber { get; set; } = 0;
        public List<GameBot> Bots { get; set; } = new List<GameBot>();

        public void Dispose()
        {
            TStuffLog.Info("Game Session is disposed");
            GameLoop?.StopLoop();
           
        }
    }
}
