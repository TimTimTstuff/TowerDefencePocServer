using ProtoBuf;
using TStuff.Game.TowerDefense3d.lib.Enums;

namespace TStuff.Game.TowerDefense3d.lib.ContractObjects
{
    [ProtoContract]
   public class MobData
    {
        [ProtoMember(1)]
        public int MobId { get; set; }
        [ProtoMember(2)]
        public string MobDisplayName { get; set; }
        [ProtoMember(3)]
        public double Speed { get; set; }
        [ProtoMember(4)]
        public ArmorType Armor;
        [ProtoMember(5)]
        public int ArmorAmount { get; set; }
        [ProtoMember(6)]
        public int XpDrop { get; set; }
        [ProtoMember(7)]
        public int GoldDrop { get; set; }
        [ProtoMember(8)]
        public int AddIncome { get; set; }
        [ProtoMember(9)]
        public int Cost { get; set; }
        [ProtoMember(10)]
        public int Hp { get; set; }
        [ProtoMember(11)]
        public int ClientModelId { get; set; }
    }
}
