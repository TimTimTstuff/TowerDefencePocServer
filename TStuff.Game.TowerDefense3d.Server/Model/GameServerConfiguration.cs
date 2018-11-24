using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TStuff.Game.TowerDefense3d.Server.Model
{
   public class GameServerConfiguration
   {
       public int ServerPort { get; set; } = 8181;
       public int EffectTicksMilliseconds { get; set; } = 1000;
       public string MainBotName { get; set; } = "Bot Matt";
       public int MaxMobSendUpdate { get; set; } = 120;
       public int LogicUpdateEachMillisecond { get; set; } = 32;
       public int SendLoopMilliseconds { get; set; } = 90;
       public int GameSessionLoopAliveMessageAfterSeconds { get; set; } = 600;
       public int ProcessNetCommandsPerUpdate { get; set; } = 50;
       public double SendStatsEachMilliseconds { get; set; } = 300;
       public int AStarMaxSearch { get; set; } = int.MaxValue;
       public int MobWaitMillisecondsToDestroyBlockingTower { get; set; } = 2000;
       public double TileDistanceDefaultTollerance { get; set; } = 0.18;
       public LogLevel ServerLogLevel { get; set; } = LogLevel.Debug;
       public LogLevel LogFileLevel { get; set; }  = LogLevel.Error;
       public string MapFileFolderPathRelativ { get; set; } = "MapImages\\";
       public double MobWaitMillisecondsBeforeUpdateRemoved { get; set; } = 2000;
       public float GameSimulationTime { get; set; } = 1000;
       public string DbMobRaceFile { get; set; } = "db\\race_mob.json";
       public string DbTowerRaceFile { get; set; } = "db\\race_tower.json";
       public string DbMobFile { get; set; } = "db\\mobs.json";
       public string DbTowerFile { get; set; } = "db\\tower.json";
   }
}
