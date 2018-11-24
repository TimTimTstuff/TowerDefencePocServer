using ProtoBuf;
using TStuff.Game.TowerDefense3d.lib.Enums;

namespace TStuff.Game.TowerDefense3d.lib.ContractObjects
{
    [ProtoContract]
    public class MapTile
   {
        [ProtoMember(1)]
       public int X;
       [ProtoMember(2)]
        public int Y;
       [ProtoMember(3)]
        public bool Block;
       [ProtoMember(4)]
        public int? BuildableByTeamId;
       [ProtoMember(5)]
        public int? TowerOwnerId;
       [ProtoMember(6)]
        public int? TowerId;
       [ProtoMember(7)]
        public int? TeamOwned;
       [ProtoMember(8)]
        public TileType Type;
   }
}
