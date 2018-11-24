using TStuff.Game.TowerDefense3d.lib.ContractObjects;

namespace TStuff.Game.TowerDefense3d.Server.Model.GameObjects
{
   public class MobHit
   {
       public ServerTowerModel SendingTower;
       public int Damage;
       public MobStatusEffectModel Status;

       public bool NoSplash { get; set; }
   }
}
