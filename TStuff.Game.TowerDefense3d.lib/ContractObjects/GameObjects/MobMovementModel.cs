using ProtoBuf;
using TStuff.Game.TowerDefense3d.lib.Enums;

namespace TStuff.Game.TowerDefense3d.lib.ContractObjects
{

    public enum MobState
    {
        Alive,
        Dead,
        Goal
    }

    [ProtoContract]
   public class MobMovementModel
    {
        [ProtoMember(1)]
        public float X { get; set; }
        [ProtoMember(2)]
        public float Y { get; set; }
        [ProtoMember(3)]
        public int Hp { get; set; }
        [ProtoMember(4)]
        public int Id { get; set; }
        [ProtoMember(5)]
        public int MobId { get; set; }
        [ProtoMember(6)]
        public float Speed { get; set; }
        [ProtoMember(7)]
        public MobStatusEffect[] Status { get; set; }
        [ProtoMember(8)]
        public MobState WorldStatus { get; set; }
    }
}
