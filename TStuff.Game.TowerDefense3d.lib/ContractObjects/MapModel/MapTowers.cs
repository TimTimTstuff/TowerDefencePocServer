using ProtoBuf;

namespace TStuff.Game.TowerDefense3d.lib.ContractObjects
{
    [ProtoContract]
    public class MapTowers
    {
        [ProtoMember(1)]
        public int TowerId { get; set; }
        [ProtoMember(2)]
        public int Id { get; set; }
        [ProtoMember(3)]
        public int X { get; set; }
        [ProtoMember(4)]
        public int Y { get; set; }

    }
}
