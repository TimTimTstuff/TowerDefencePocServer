using ProtoBuf;
using TStuff.Game.TowerDefense3d.lib.Enums;

namespace TStuff.Game.TowerDefense3d.lib.ContractObjects
{
    [ProtoContract]
   public class MapRequests
    {
        [ProtoMember(1)]
        public MapRequestTypes Request { get; set; }
    }
}
