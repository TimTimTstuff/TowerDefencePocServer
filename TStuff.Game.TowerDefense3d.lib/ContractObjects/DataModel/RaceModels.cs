using ProtoBuf;

namespace TStuff.Game.TowerDefense3d.lib.ContractObjects
{
    [ProtoContract]
   public class RaceModels
    {
        [ProtoMember(1)]
        public MobRace[] MobRaces { get; set; }
        [ProtoMember(2)]
        public TowerRace[] TowerRaces { get; set; }
        [ProtoMember(3)]
        public MobData[] Mobs { get; set; }
        [ProtoMember(4)]
        public TowerData[] Towers { get; set; }
    }
}
