using ProtoBuf;
using TStuff.Game.TowerDefense3d.lib.Enums;

namespace TStuff.Game.TowerDefense3d.lib.ContractObjects
{
    [ProtoContract]
    public class MobStatusEffectModel
    {
        [ProtoMember(1)]
        public MobStatusEffect Type { get; set; }
        [ProtoMember(2)]
        public int Value { get; set; }
        [ProtoMember(4)]
        public double LeftDuration { get; set; }

        public int Duration { get; set; }
        public int SenderTowerId { get; set; }
        public double TimeSinceLastTick { get; set; }
    }
}
