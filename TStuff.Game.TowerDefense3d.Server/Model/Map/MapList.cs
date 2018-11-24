using System.Collections.Generic;
using System.Linq;
using TStuff.Game.TowerDefense3d.lib.ContractObjects;

namespace TStuff.Game.TowerDefense3d.Server.Model.Map
{
   public class MapList
   {
       private List<MapModel> _mapList = new List<MapModel>();
       public List<MapModel> Maps
       {
           get { return _mapList; } 
           set { _mapList = value; }
       }


       public MapInfo[] GetMapListNetwork()
       {
           return _mapList.Select(m => new MapInfo
           {
               Id = m.Id,
               Height = m.Rows,
               Width = m.Cols,
               Description = m.Description,
               MaxPlayer = m.Teams,
               MapName = m.Name
           }).ToArray();
       }

       public MapInfo GetMapInfoById(int selectedMapId)
       {
           return _mapList.Where(m=>m.Id == selectedMapId).Select(m => new MapInfo
           {
               Id = m.Id,
               Height = m.Rows,
               Width = m.Cols,
               Description = m.Description,
               MaxPlayer = m.Teams,
               MapName = m.Name
           }).FirstOrDefault();
        }
   }
}
