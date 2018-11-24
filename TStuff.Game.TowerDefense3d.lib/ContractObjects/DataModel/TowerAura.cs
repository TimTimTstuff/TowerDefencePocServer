using ProtoBuf;

namespace TStuff.Game.TowerDefense3d.lib.ContractObjects
{
    [ProtoContract]
   public class TowerAura
    {
        [ProtoMember(1)]
        public AuraType Type { get; set; }
        [ProtoMember(2)]
        public float Multiplier { get; set; }
        [ProtoMember(3)]
        public int TowerId { get; set; }
        

    }
}
