using ProtoBuf;

namespace TStuff.Game.TowerDefense3d.lib.ContractObjects
{
    [ProtoContract]
   public class ClientMessage
    {
        [ProtoMember(1)] public string Title;
        [ProtoMember(2)] public string Message;
        [ProtoMember(3)] public MessageType MessageType;
        [ProtoMember(4)] public int id;
    }
}
