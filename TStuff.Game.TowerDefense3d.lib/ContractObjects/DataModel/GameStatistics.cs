using ProtoBuf;

namespace TStuff.Game.TowerDefense3d.lib.ContractObjects
{
    [ProtoContract]
  public class GameStatistics
    {
        [ProtoMember(1)]
        public PlayerStatistics[] Player { get; set; }
    }
}
