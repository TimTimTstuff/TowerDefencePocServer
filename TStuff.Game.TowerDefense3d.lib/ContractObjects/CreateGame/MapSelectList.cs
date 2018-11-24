using ProtoBuf;

namespace TStuff.Game.TowerDefense3d.lib.ContractObjects
{
    [ProtoContract]
   public class MapSelectList
    {
        [ProtoMember(1)]
        public MapInfo[] Maps;
    }
}
