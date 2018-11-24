using TStuff.Game.TowerDefense3d.lib.Enums;
using TStuff.Game.TowerDefense3d.Server.Managers;

namespace TStuff.Game.TowerDefense3d.Server.Model
{
  

   public class GameUser
   {
       public User User { get; set; }
       public PlayerState State { get; set; }
       public bool Owner { get; set; }
       public int Money { get; set; }
       public int Income { get; set; }
       public int Kills { get; set; }
       public int MobsSend { get; set; }
       public int Xp { get; set; }
       public int TeamId { get; set; }
   }
}
