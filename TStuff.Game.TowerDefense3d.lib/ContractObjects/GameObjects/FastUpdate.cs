using ProtoBuf;

namespace TStuff.Game.TowerDefense3d.lib.ContractObjects
{
    [ProtoContract]
    public class FastUpdate
    {
        [ProtoMember(1)]
        public long TimeFrame { get; set; }
        [ProtoMember(2)]
        public MobMovementModel[] Mobs { get; set; }
        [ProtoMember(3)]
        public TowerStateModel[] Tower { get; set; }
    }
}
