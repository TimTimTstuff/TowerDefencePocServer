using ProtoBuf;

namespace TStuff.Game.TowerDefense3d.lib.ContractObjects
{
    [ProtoContract]
    public class PlayerStatistics
    {
        [ProtoMember(1)] public int Kills { get; set; }
        [ProtoMember(2)] public int Xp { get; set; }
        [ProtoMember(3)] public int GoldTotal { get; set; }
        [ProtoMember(4)] public int TowerBuildTotal { get; set; }
        [ProtoMember(5)] public int TowerDestroyed { get; set; }
        [ProtoMember(6)] public int MobsSend { get; set; }
        [ProtoMember(7)] public int GoldForTowers { get; set; }
        [ProtoMember(8)] public int GoldForMobs { get; set; }
    }
}