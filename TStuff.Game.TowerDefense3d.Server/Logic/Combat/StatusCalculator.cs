using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TStuff.Game.TowerDefense3d.lib.ContractObjects;
using TStuff.Game.TowerDefense3d.lib.Enums;
using TStuff.Game.TowerDefense3d.Server.Managers;
using TStuff.Game.TowerDefense3d.Server.Model;
using TStuff.Game.TowerDefense3d.Server.Model.GameObjects;

namespace TStuff.Game.TowerDefense3d.Server.Logic.Combat
{
    public static class StatusCalculator
    {
        public static int EffectTicksMilliseconds = GConfig.D.EffectTicksMilliseconds; //1000;
       
        public static void CalculateLeftTime(List<MobStatusEffectModel> effects, double delta)
        {
            effects?.ForEach(se =>
            {
                se.LeftDuration -= delta;
               
            });

            
        }

        public static List<MobStatusEffectModel> GetCleardEffectList( List<MobStatusEffectModel> effects)
        {
            return effects?.Where(x => x.LeftDuration > 0).ToList();
        }

        public static double GetSpeedValueByStatus(MobData mob, List<MobStatusEffectModel> effects)
        {
            if (effects == null) return mob.Speed;
            if (effects.Any(se => se.Type == MobStatusEffect.Stun))
            {
                return 0;
            }
            //Only use hightest Slowingeffect
            if (effects.Any(se => se.Type == MobStatusEffect.Slowed))
            {
                var slow = effects.Max(se => se.Value);
                return Math.Round(mob.Speed - mob.Speed * (slow / 100f),2);
            }
            return mob.Speed;
        }

        public static void CalculateHot(MobData mob, List<MobStatusEffectModel> effects, double delta)
        {
            if (effects == null) return;

            if (effects.Any(se => se.Type == MobStatusEffect.Hot))
            {
                effects.Where(se=>se.Type == MobStatusEffect.Hot).ToList().ForEach(se =>
                {
                    if (se.TimeSinceLastTick <= GConfig.D.EffectTicksMilliseconds)
                    {
                        se.TimeSinceLastTick += delta;
                        return;
                    }
                    mob.Hp += se.Value;
                    se.TimeSinceLastTick = 0;
                });
            }
        }

        
        public static void CalculateDot(MobData mob, List<MobStatusEffectModel> effects, double delta,Action<int> towerkilledCallback)
        {
            if (effects == null) return;

            if (effects.Any(se => se.Type == MobStatusEffect.Dot))
            {
                effects.Where(se => se.Type == MobStatusEffect.Dot).ToList().ForEach(se =>
                {
                    if (se.TimeSinceLastTick <= EffectTicksMilliseconds)
                    {
                        se.TimeSinceLastTick += delta;
                        return;
                    }
                    mob.Hp -= se.Value;
                    if (mob.Hp <= 0)
                    {
                        if(towerkilledCallback != null)
                            towerkilledCallback(se.SenderTowerId);
                        else
                            TStuffLog.Debug("Tower Killed Callback is not Set");
                    }
                    se.TimeSinceLastTick = 0;
                });
            }
        }

        public static int GetReducedArmorByStatus(MobData mob, List<MobStatusEffectModel> effects)
        {
            //Only use max value
            if (effects == null || mob.ArmorAmount == 0 || effects.All(a => a.Type != MobStatusEffect.ArmoreReduced)) return mob.ArmorAmount;
            var reduced = effects.Where(a=>a.Type == MobStatusEffect.ArmoreReduced).Max(a => a.Value);
            return (int) Math.Round(mob.ArmorAmount - (mob.ArmorAmount*(reduced/100f)));
        }
    }
}
