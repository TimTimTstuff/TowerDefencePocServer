using ProtoBuf;

namespace TStuff.Game.TowerDefense3d.lib.ContractObjects
{
    [ProtoContract]
    public  class GameCommands
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(6)]
        public int IntVal { get; set; }
        [ProtoMember(2)]
        public int X { get; set; }
        [ProtoMember(3)]
        public int Y { get; set; }
        [ProtoMember(4)]
        public string AddContent { get; set; }
        [ProtoMember(5)]
        public GameCommand Command;

        public int SenderId;
    }
}
