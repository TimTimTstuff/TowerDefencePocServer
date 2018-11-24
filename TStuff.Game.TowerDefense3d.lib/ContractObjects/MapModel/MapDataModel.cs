



using ProtoBuf;

namespace TStuff.Game.TowerDefense3d.lib.ContractObjects
{
    [ProtoContract]
   public class MapDataModel
    {
        [ProtoMember(1)]
        public MapTile[] Data { get; set; } 
        
    }
}
