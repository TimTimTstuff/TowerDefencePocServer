using ProtoBuf;

namespace TStuff.Game.TowerDefense3d.lib.ContractObjects
{
    [ProtoContract]
   public class TowerRace
    {
        [ProtoMember(1)]
        public int RaceId { get; set; }
        [ProtoMember(2)]
        public string DisplayName { get; set; }
        [ProtoMember(3)]
        public string Description { get; set; }
        [ProtoMember(4)]
        public string Towers { get; set; }
        
    }
}
