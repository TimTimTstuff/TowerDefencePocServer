using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TStuff.Game.TowerDefense3d.lib.ContractObjects;
using TStuff.Game.TowerDefense3d.Server.Model.Map;

namespace TStuff.Game.TowerDefense3d.Server.Managers
{
   public class GlobalRegister
   {
       public GlobalRegister()
       {

           User = new UserHandler();
           MapListManager = new MapList();
           TowerRaces = new List<TowerRace>();
           MobRaces = new List<MobRace>();
           Mobs = new List<MobData>();
           Towers = new List<TowerData>();
           GameSessions = new GameSessionManager(this);
           LoadMaps();
           LoadDatabase();
        }

       private  void LoadDatabase()
       {
           MobRaces = JsonConvert.DeserializeObject<List<MobRace>>(File.ReadAllText(GConfig.D.DbMobRaceFile));
          TowerRaces = JsonConvert.DeserializeObject<List<TowerRace>>(File.ReadAllText(GConfig.D.DbTowerRaceFile));
           Mobs = JsonConvert.DeserializeObject<List<MobData>>(File.ReadAllText(GConfig.D.DbMobFile));
           Towers = JsonConvert.DeserializeObject<List<TowerData>>(File.ReadAllText(GConfig.D.DbTowerFile));
          
       }

       private void DemoData()
       {
           var holder = new MobHolder
           {
               MobData = Mobs
           };
           var holder1 = new TowerHolder()
           {
               TowerData = Towers
           };
           var holder2 = new TowerRaceHolder()
           {
               TowerRace = TowerRaces
           };
           var holder3 = new MobRaceHolder()
           {
               MobRace = MobRaces
           };

           File.WriteAllText("db\\1.race_mob.json", JsonConvert.SerializeObject(holder3));
           File.WriteAllText("db\\1.mob.json", JsonConvert.SerializeObject(holder));
           File.WriteAllText("db\\1.race_tower.json", JsonConvert.SerializeObject(holder2));
           File.WriteAllText("db\\1.tower.json", JsonConvert.SerializeObject(holder1));
       }

       private void  LoadMaps()
       {
           var files = Directory.GetFiles(GConfig.D.MapFileFolderPathRelativ).Where(f => f.EndsWith(".json")).ToList();

           var maps = new List<MapModel>();
           files.ForEach(f =>
           {
               var mapData = JsonConvert.DeserializeObject<MapModel>(File.ReadAllText(f));
               maps.Add(mapData);
           });

           MapListManager.Maps = maps;
           var x = 0;
           foreach (var game in MapListManager.Maps)
           {
               game.Id = x;
               x++;
           }
        }

        public MapList MapListManager;
       public UserHandler User;
       public GameSessionManager GameSessions;
       public List<MobRace> MobRaces;
       public List<TowerRace> TowerRaces;
       public List<TowerData> Towers;
       public List<MobData> Mobs;

   }
}
