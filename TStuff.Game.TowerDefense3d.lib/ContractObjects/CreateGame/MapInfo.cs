using ProtoBuf;

namespace TStuff.Game.TowerDefense3d.lib.ContractObjects
{
    [ProtoContract]
    public class MapInfo
    {
        [ProtoMember(1)]
        public string MapName { get; set; }
        [ProtoMember(2)]
        public int MaxPlayer { get; set; }
        [ProtoMember(3)]
        public int Width { get; set; }
        [ProtoMember(4)]
        public int Height { get; set; }
        [ProtoMember(5)]
        public string Description { get; set; }
        [ProtoMember(6)]
        public int Id { get; set; }
    }
}