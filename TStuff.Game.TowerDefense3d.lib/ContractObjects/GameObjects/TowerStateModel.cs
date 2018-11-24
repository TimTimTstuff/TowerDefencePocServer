using ProtoBuf;

namespace TStuff.Game.TowerDefense3d.lib.ContractObjects
{
    [ProtoContract]
  public  class TowerStateModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public int[] TargetId { get; set; }
        [ProtoMember(3)]
        public AuraType[] TowerAura { get; set; }
    }
}
