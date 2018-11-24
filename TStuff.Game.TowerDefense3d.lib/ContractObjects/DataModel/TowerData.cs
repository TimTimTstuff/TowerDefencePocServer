using ProtoBuf;
using TStuff.Game.TowerDefense3d.lib.Enums;

namespace TStuff.Game.TowerDefense3d.lib.ContractObjects
{
    [ProtoContract]
   public class TowerData
    {
        [ProtoMember(1)]
        public int TowerId { get; set; }
        [ProtoMember(2)]
        public string DisplayName { get; set; }
        [ProtoMember(3)]
        public int Range { get; set; }
        [ProtoMember(4)]
        public int DamageMin { get; set; }
        [ProtoMember(5)]
        public int DamageMax { get; set; }
        [ProtoMember(6)]
        public int MaxTargets { get; set; }
        [ProtoMember(7)]
        public TowerDamageType DmgType { get; set; }
        [ProtoMember(8)]
        public double FireSpeed { get; set; }
        [ProtoMember(9)]
        public int AttackId { get; set; }
        [ProtoMember(10)]
        public int ClientModelId { get; set; }
        [ProtoMember(11)]
        public int GoldCost { get; set; }
        [ProtoMember(12)]
        public string Description { get; set; }
        [ProtoMember(13)]
        public int SplashRange { get; set; }
        [ProtoMember(14)]
        public int SplashDamage { get; set; }
        [ProtoMember(15)]
        public double CritChance { get; set; }
        [ProtoMember(16)]
        public double CritMultiplier { get; set; }
        [ProtoMember(17)]
        public double EffectChance { get; set; }
        [ProtoMember(18)]
        public MobStatusEffectModel Effect { get; set; }
        [ProtoMember(19)]
        public TowerAura Aura { get; set; }
        [ProtoMember(20)]
        public int AuraRange { get; set; }
    }
}
