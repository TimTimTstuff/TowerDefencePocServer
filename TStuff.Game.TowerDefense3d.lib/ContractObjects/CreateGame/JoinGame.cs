using ProtoBuf;
using TStuff.Game.TowerDefense3d.lib.Enums;

namespace TStuff.Game.TowerDefense3d.lib.ContractObjects
{
    [ProtoContract]
   public class JoinGame
    {
        [ProtoMember(1)]
        public int GameId { get; set; }
        [ProtoMember(2)]
        public Player[] PlayerInfo { get; set; }
        [ProtoMember(3)]
        public GameState GameState { get; set; }
        [ProtoMember(4)]
        public MapInfo MapInfo { get; set; }
    }
}
