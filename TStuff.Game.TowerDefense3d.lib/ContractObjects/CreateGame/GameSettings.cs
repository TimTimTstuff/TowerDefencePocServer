using ProtoBuf;

namespace TStuff.Game.TowerDefense3d.lib.ContractObjects
{
    [ProtoContract]
   public class GameSettings
    {
        [ProtoMember(1)]
        public int MapId { get; set; }
        [ProtoMember(2)]
        public int Hp { get; set; }
        [ProtoMember(3)]
        public int MaxMobs { get; set; }
        [ProtoMember(4)]
        public int IncomeTime { get; set; }
        [ProtoMember(5)]
        public int MobSpawnDelay { get; set; }
        [ProtoMember(6)]
        public int StartGold { get; set; }
        [ProtoMember(7)]
        public int StartIncome { get; set; }
        [ProtoMember(8)]
        public int TimeBeforeMobs { get; set; }
        [ProtoMember(9)]
        public int MaxMobsPerTeam { get; set; }
        [ProtoMember(10)]
        public bool ShareIncome { get; set; }
        [ProtoMember(11)]
        public bool IncomeVisibleToAll { get; set; }
        [ProtoMember(12)]
        public bool LifeSteal { get; set; }
        [ProtoMember(13)]
        public bool SendRandomLine { get; set; }
       
    }
}
