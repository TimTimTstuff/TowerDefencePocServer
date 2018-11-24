using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TStuff.Game.TowerDefense3d.lib.ContractObjects;
using TStuff.Game.TowerDefense3d.lib.Enums;
using TStuff.Game.TowerDefense3d.Server.Model.GameObjects;

namespace TStuff.Game.TowerDefense3d.Server.Logic.Combat
{
    public class DamageCalculator
    {
        private static readonly Dictionary<TowerDamageType, Dictionary<ArmorType, int>> _calcValues = new Dictionary<TowerDamageType, Dictionary<ArmorType, int>>
       {
            {TowerDamageType.Normal,new Dictionary<ArmorType,int>
            {
                {ArmorType.Light,100},
                { ArmorType.Medium, 150},
                { ArmorType.Heavy,100},
                { ArmorType.Fortified,70},
                { ArmorType.Hero,  100},
                { ArmorType.Unarmored,100}
            }},
            {TowerDamageType.Pierce,new Dictionary<ArmorType,int>
            {
                {ArmorType.Light,200},{ArmorType.Medium, 75}, { ArmorType.Heavy,100}, {ArmorType.Fortified,35},{ArmorType.Hero,  50}, {ArmorType.Unarmored, 150}
            
            }},
            {TowerDamageType.Siege,new Dictionary<ArmorType,int>{ {ArmorType.Light,100},{ArmorType.Medium, 50}, { ArmorType.Heavy,100}, {ArmorType.Fortified,150},{ArmorType.Hero, 50}, {ArmorType.Unarmored, 150}}},
            {TowerDamageType.Magic,new Dictionary<ArmorType,int>{ {ArmorType.Light,125},{ArmorType.Medium, 75}, { ArmorType.Heavy,200}, {ArmorType.Fortified,35},{ArmorType.Hero,  50}, {ArmorType.Unarmored, 100}}},
            {TowerDamageType.Chaos,new Dictionary<ArmorType,int>{ {ArmorType.Light,100},{ArmorType.Medium, 100}, {ArmorType.Heavy,100}, {ArmorType.Fortified,100},{ArmorType.Hero, 100}, {ArmorType.Unarmored,100}}},
            {TowerDamageType.Spells,new Dictionary<ArmorType,int>{ {ArmorType.Light,100},{ArmorType.Medium, 100}, {ArmorType.Heavy,100}, {ArmorType.Fortified,100},{ArmorType.Hero, 70}, {ArmorType.Unarmored, 100}}},
            {TowerDamageType.Hero,new Dictionary<ArmorType,int>{ {ArmorType.Light,100},{ArmorType.Medium, 100}, {ArmorType.Heavy,100}, {ArmorType.Fortified,50},{ArmorType.Hero,  100}, {ArmorType.Unarmored,100}}}
       };

        private static Random _random =
        new Random();

        public static int GetDamageByArmorAndAttack(int baseDamage, ArmorType armor, TowerDamageType damageType)
        {
            var dmgValuePercent = _calcValues[damageType][armor]/100f;
            return (int)Math.Round(baseDamage*(dmgValuePercent));
        }

        public static int ReduceDamageByArmor(int baseDamage, int armor)
        {
            return (int) Math.Round(baseDamage- baseDamage*((armor/100f)));
        }

        public static int GetTowerDamage(TowerData tower)
        {
            return _random.Next(tower.DamageMin, tower.DamageMax + 1);

        }

        public static int GetTowerDamageForMob(TowerData tower, MobData mob, List<MobStatusEffectModel> effectList)
        {
            
            var dmg = ReduceDamageByArmor(GetDamageByArmorAndAttack(GetTowerDamage(tower), mob.Armor, tower.DmgType), StatusCalculator.GetReducedArmorByStatus(mob,effectList));
            if (dmg <= 0)
            {
                return 1;
            }
            return dmg;
        }

        public static int CalculateDamageForSplash(TowerData tower, MobData mob, List<MobStatusEffectModel> effectList)
        {
            var dmg = ReduceDamageByArmor(GetDamageByArmorAndAttack(tower.SplashDamage, mob.Armor, tower.DmgType), StatusCalculator.GetReducedArmorByStatus(mob, effectList));
            if (dmg <= 0)
            {
                return 1;
            }
            return dmg;
        }
    }
}
