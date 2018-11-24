using ProtoBuf;
using TStuff.Game.TowerDefense3d.lib.Enums;

namespace TStuff.Game.TowerDefense3d.lib.ContractObjects
{
    [ProtoContract]
    public class GameNotifications
    {
        [ProtoMember(1)]
        public GameNotificationType Notification { set; get; }
        [ProtoMember(2)]
        public int IntValue { get; set; }
        [ProtoMember(3)]
        public string StringValue { get; set; }
    }
}
