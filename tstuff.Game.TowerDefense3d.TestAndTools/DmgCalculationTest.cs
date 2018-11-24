using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TStuff.Game.TowerDefense3d.lib.ContractObjects;
using TStuff.Game.TowerDefense3d.lib.Enums;
using TStuff.Game.TowerDefense3d.Server;
using TStuff.Game.TowerDefense3d.Server.Logic.Combat;
using TStuff.Game.TowerDefense3d.Server.Model.GameObjects;
using Xunit;
using Xunit.Abstractions;

namespace tstuff.Game.TowerDefense3d.TestAndTools
{
   public class DmgCalculationTest
    {

        private readonly ITestOutputHelper output;

        public DmgCalculationTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
       public void DmgArmorTypeCalc()
       {
            /* INFO
             * 		        Light	Medium	Heavy	Fort	Hero	Unarmored
                    Normal	100%	150%	100%	70%		100%	100%
                    Pierce	200%	75%		100%	35%		50%		150%
                    Siege	100%	50%		100%	150%	50%		150%
                    Magic	125%	75%		200%	35%		50%		100%
                    Chaos	100%	100%	100%	100%	100%	100%
                    Spells	100%	100%	100%	100%	70%		100%
                    Hero	100%	100%	100%	50%		100%	100%
             */
            var first = DamageCalculator.GetDamageByArmorAndAttack(100, ArmorType.Heavy, TowerDamageType.Normal);
            Assert.Equal(100,first);
            var second = DamageCalculator.GetDamageByArmorAndAttack(100, ArmorType.Light, TowerDamageType.Pierce);
            Assert.Equal(200,second);
            var third = DamageCalculator.GetDamageByArmorAndAttack(100, ArmorType.Medium, TowerDamageType.Magic);
            Assert.Equal(75,third);

       }

       [Fact]
       public void DamageReduction()
       {
           var first = DamageCalculator.ReduceDamageByArmor(100, 10);
           Assert.Equal(90,first);
           var second = DamageCalculator.ReduceDamageByArmor(100, 33);
            Assert.Equal(67,second);

       }

       [Fact]
       public void ValidDamageForMob()
       {
           var md = new MobData
           {
               Armor = ArmorType.Light,
               ArmorAmount = 10
           };

            var td = new TowerData
            {
                DamageMax = 100,
                DamageMin = 90,
                DmgType = TowerDamageType.Pierce
            };

            for (var i = 0; i <= 50; i++)
           {
               var damage = DamageCalculator.GetTowerDamageForMob(td, md,new List<MobStatusEffectModel>());
                Assert.InRange(damage, 162, 180);
               //output.WriteLine(damage.ToString());
            }

       }

       [Fact]
       public void StunAndSlow()
       {
            
       }

       [Fact]
       public void StatuseffectDuration()
       {
           var el = DemoEffectModel();
            //Durratino
            StatusCalculator.CalculateLeftTime(el,10);
           Assert.Equal(90,el[0].LeftDuration);
           Assert.Equal(290,el[1].LeftDuration);
            

       }

       [Fact]
       public void CalcDot()
       {

           GConfig.D.EffectTicksMilliseconds = 1000;
           var el = DemoEffectModel();
           var dMob = new MobData {Hp = 21};
            StatusCalculator.CalculateDot(dMob,el,290, i =>
           {
               Assert.True(false,"Shouldnt trigger killCallback 1");
           });

            Assert.Equal(21,dMob.Hp);
            Assert.Equal(590,el[1].TimeSinceLastTick);

            el = DemoEffectModel();
             dMob = new MobData { Hp = 21 };
           el[1].TimeSinceLastTick = 1001;
            StatusCalculator.CalculateDot(dMob, el, 701, i =>
            {
                Assert.True(false, "Shouldnt trigger killCallback 2");
            });
            Assert.Equal(1, dMob.Hp);
            Assert.Equal(0, el[1].TimeSinceLastTick);

            var killtrigger = false;
            el = DemoEffectModel();
            el[1].TimeSinceLastTick = 1001;
            dMob = new MobData { Hp = 20};
            StatusCalculator.CalculateDot(dMob, el, 10101, i =>
            {
                Assert.Equal(2,i);
                killtrigger = true;
            });
            Assert.True(killtrigger);
            Assert.Equal(0, dMob.Hp);
            Assert.Equal(0, el[1].TimeSinceLastTick);


        }

       [Fact]
       public void CalcSlowStun()
       {
           var el = DemoEffectModel();
          var val =  StatusCalculator.GetSpeedValueByStatus(new MobData {Speed = 100}, el);
           Assert.Equal(0,val);
           el.Remove(el[4]);
           val = StatusCalculator.GetSpeedValueByStatus(new MobData { Speed = 100 }, el);
            Assert.Equal(60, val);
        }

        [Fact]
       public void CalcArmoreReduce()
        {
            var el = DemoEffectModel();
            var res = StatusCalculator.GetReducedArmorByStatus(new MobData {ArmorAmount = 100}, el);
            Assert.Equal(90,res);
        }


        private static List<MobStatusEffectModel> DemoEffectModel()
       {
           return new List<MobStatusEffectModel>
           {
               new MobStatusEffectModel
               {
                   SenderTowerId = 1,
                   Value = 10,
                   Type = MobStatusEffect.ArmoreReduced,
                   LeftDuration = 100,
                   TimeSinceLastTick = 100,
                   Duration = 1000
               },
               new MobStatusEffectModel
               {
                   SenderTowerId = 2,
                   Value = 20,
                   Type = MobStatusEffect.Dot,
                   LeftDuration = 300,
                   TimeSinceLastTick = 300,
                   Duration = 2000
                   
               },
               new MobStatusEffectModel
               {
                   SenderTowerId = 3, 
                   LeftDuration = 500,
                   Value = 30,
                   Type = MobStatusEffect.Hot,
                   Duration = 3000,
                   TimeSinceLastTick = 500
               },
               new MobStatusEffectModel
               {
                   SenderTowerId =  4,
                   LeftDuration = 800,
                   Value = 40,
                   Type = MobStatusEffect.Slowed,
                   Duration = 4000,
                   TimeSinceLastTick = 1000
               },
               new MobStatusEffectModel
               {
                   LeftDuration = 1100,
                   Value = 50,
                   Type = MobStatusEffect.Stun,
                   Duration = 5000,
                   TimeSinceLastTick = 1200,
                   SenderTowerId = 5
               }
           };
       }
    }
}
