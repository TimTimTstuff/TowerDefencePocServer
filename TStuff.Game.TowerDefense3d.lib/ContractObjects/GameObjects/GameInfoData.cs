using ProtoBuf;

namespace TStuff.Game.TowerDefense3d.lib.ContractObjects
{
    [ProtoContract]
   public class GameInfoData
    {
        [ProtoMember(1)]
        public int MobsOnWorld { get; set; }
        [ProtoMember(2)]
        public int HPTeam2 { get; set; }
        [ProtoMember(3)]
        public int OwneMoney { get; set; }
        [ProtoMember(4)]
        public int OwneIncome { get; set; }
        [ProtoMember(5)]
        public int IncomeTimer { get; set; }
        [ProtoMember(6)]
        public int HPTeam1 { get; set; }
        [ProtoMember(7)]
        public int HPTeam3 { get; set; }
        [ProtoMember(8)]
        public int HPTeam4 { get; set; }
        [ProtoMember(9)]
        public int TeamIncomeTotal { get; set; }
        [ProtoMember(10)]
        public int HighestIncome { get; set; }
        [ProtoMember(11)]
        public string[] AllIncome { get; set; }
    }
}
