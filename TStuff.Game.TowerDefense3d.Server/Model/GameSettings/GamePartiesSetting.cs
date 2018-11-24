namespace TStuff.Game.TowerDefense3d.Server.Model
{
   public class GamePartiesSetting
    {
        public int MaxPlayers { get; set; }
        public int RoundSeconds { get; set; }
        public int StartGoldRound { get; set; }
        public int TimeToStart { get; set; }
        public int MaxMobs { get; set; }
        public int MapId { get; set; }
        public int Lives { get; set; }
        public int Teams { get; set; }
        public int MaxMobsPerTeam { get; set; }
        public int StartGold { get; set; }
        public int MobSpawnDelay { get; set; }
        public int TowerSellMoney { get; set; }
        public bool SpawnOnAll { get; set; }
    }
}
