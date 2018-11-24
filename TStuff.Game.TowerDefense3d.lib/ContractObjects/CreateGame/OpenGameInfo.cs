using ProtoBuf;

namespace TStuff.Game.TowerDefense3d.lib.ContractObjects
{
    [ProtoContract]
   public class OpenGameInfo
    {
        [ProtoMember(1)]
        public string Owner { get; set; }
        [ProtoMember(2)]
        public MapInfo MapName { get; set; }
        [ProtoMember(3)]
        public int Players { get; set; }
        [ProtoMember(4)]
        public int GameId { get; set; }
    }
}
