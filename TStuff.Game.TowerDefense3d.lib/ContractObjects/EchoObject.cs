

using ProtoBuf;

namespace TStuff.Game.TowerDefense3d.lib.ContractObjects
{
    [ProtoContract]
    public class EchoObject
    {
        [ProtoMember(1)]
       public int Id { get; set; }

        [ProtoMember(2)]
        public string Message { get; set; }
    }
}
