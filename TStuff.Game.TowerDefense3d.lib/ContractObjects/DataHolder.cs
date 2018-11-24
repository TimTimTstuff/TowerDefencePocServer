using System;
using System.Collections.Generic;
using System.Text;

namespace TStuff.Game.TowerDefense3d.lib.ContractObjects
{
   public class MobHolder
    {
       public List<MobData> MobData { get; set; }
    }

    public class TowerHolder
    {
        public  List<TowerData> TowerData { get; set; }
    }

    public class TowerRaceHolder
    {
        public List<TowerRace> TowerRace { get; set; }
    }

    public class MobRaceHolder
    {
        public List<MobRace> MobRace { get; set; }
    }
}
