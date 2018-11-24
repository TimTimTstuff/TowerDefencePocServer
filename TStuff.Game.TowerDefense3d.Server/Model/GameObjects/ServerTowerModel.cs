using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using TStuff.Game.TowerDefense3d.lib;
using TStuff.Game.TowerDefense3d.lib.ContractObjects;
using TStuff.Game.TowerDefense3d.Server.Logic;
using TStuff.Game.TowerDefense3d.Server.Logic.Combat;

namespace TStuff.Game.TowerDefense3d.Server.Model.GameObjects
{
   public class ServerTowerModel
   {
       public List<ServerMob> Targets = new List<ServerMob>();
       public  TowerData TowerData { get; }
        public GameUser Owner { get; set; }

       private double _fireCooldown = 0;
       private readonly Random _random;
       private readonly GameSession _session;

       #region RuntimeInfo
       public int TowerId { get; set; }
       public int InstanceId { get; set; }
       public int X { get; set; }
       public int Y { get; set; }
       public bool IsFire { get; set; }
       public int Kills { get; set; }
       public int Xp { get; set; }
       public double LastUpdate { get; set; }
        public List<TowerAura> Auren { get; set; } = new List<TowerAura>();
       public bool hasChanged;
       #endregion


       public ServerTowerModel(GameUser owner,TowerData towerData, int x, int y,GameSession session)
       {
           InstanceId = InstanceHelper.GetNewInstanceId();
           _session = session;
           TowerData = towerData;
           Targets = new List<ServerMob>();
           TowerId = TowerData.TowerId;
           Owner = owner;
            _random = new Random(InstanceId);
            Auren = new List<TowerAura>();
           X = x;
           Y = y;
           if (TowerData.Aura != null)
           {
               TowerData.Aura.TowerId = InstanceId;
               ApplieTowerAura();
            }
           hasChanged = true;

       }

       public void Update(double delta)
       {
            var targetToRemove = Targets.Where(a => a.IsDead || !IsInDistance(X, Y, a.X, a.Y)).ToList();
            targetToRemove.ForEach(w =>
            {
                Targets.Remove(w);
                hasChanged = true;
            });

           if (_fireCooldown > 0)
               _fireCooldown -= delta;

           if (Targets.Count < TowerData.MaxTargets)
           {
               try
               {
                   SearchTarget();
               }
               catch (Exception ex)
               {
                   TStuffLog.Error(ex.ToString());
               }
           }
       
           Targets.ForEach(t =>
           {

               if (_fireCooldown <= 0)
               {
                   _fireCooldown = TowerData.FireSpeed;
                   AttackTarget(t);
               }
               IsFire = true;
               hasChanged = true;
           });

           if (Targets.Count == 0)
           {
               IsFire = false;
               hasChanged = true;
           }
       }

       public void ApplieTowerAura()
       {
           if (TowerData.AuraRange > 0 && TowerData.Aura != null)
           {
               var tower = _session.GameTower.Where(x => x.TowerInDistance(this, TowerData.AuraRange)).ToList();
               tower.ForEach(t =>
               {
                   t.AddAura(TowerData.Aura);
               });
           }
       }

       public void RemoveTowerAura()
       {
           if (TowerData.AuraRange > 0 && TowerData.Aura != null)
           {
               var tower = _session.GameTower.Where(x => x.TowerInDistance(this, TowerData.AuraRange)).ToList();
               tower.ForEach(t =>
               {
                   t.RemoveAura(TowerData.Aura);
               });
           }
        }

       private void RemoveAura(TowerAura towerDataAura)
       {
           if (Auren.All(t => t.TowerId != towerDataAura.TowerId)) return;
           Auren.Remove(towerDataAura);
           hasChanged = true;
       }

       private void AddAura(TowerAura towerDataAura)
       {
           if (Auren.Any(t => t.TowerId == towerDataAura.TowerId)) return;
           Auren.Add(towerDataAura);
           hasChanged = true;
        }

     

       private void SearchTarget()
       {
            

          var t = _session.GameMobs.ToArray();
            var m = t.FirstOrDefault(a => a.TargetTeam == Owner.TeamId && IsInDistance(X, Y, a.X, a.Y));
           if (m != null)
           {
                  Targets.Add(m);
               hasChanged = true;
            }

       }
  public bool TowerInDistance(ServerTowerModel tower,int distance)
       {
           return GetDistance(tower)<=distance;
       }

        public bool MobInDistance(ServerMob mob, int distance)
        {
            return GetDistance(mob) <= distance;
        }

      
        public double GetDistance(ServerMob mob)
       {
           return GetDistance(mob.X, mob.Y);
       }

       public double GetDistance(ServerTowerModel tower)
       {
           return GetDistance(tower.X, tower.Y);

       }

       public double GetDistance(int argx, int argy)
       {
            var dist = Math.Sqrt(Math.Pow(X - argx, 2) + Math.Pow(Y - argy, 2));
            if (dist < 0) dist = dist * -1;
            return dist;
        }

       private bool IsInDistance(int x, int y, int argX, int argY)
       {
           var dist = Math.Sqrt(Math.Pow(x - argX, 2) + Math.Pow(y-argY, 2));
           if (dist < 0)dist = dist * -1;
           return dist <= TowerData.Range;
       }

       private void AttackTarget(ServerMob target)
       {
            //effect by chance
           var effect = _random.Next(0, 100) <= TowerData.EffectChance;
           var hit = new MobHit
           {
               Damage = DamageCalculator.GetTowerDamageForMob(TowerData,target.MobData,target.StatusEffect),
               SendingTower = this,
               Status = effect?TowerData.Effect:null
           };
            target.Hit(hit);
            
       }

       public void SellTower()
       {
           var backMoney = TowerData.GoldCost / 100 * _session.Setting.TowerSellMoney;
           Owner.Money +=  backMoney;
            RemoveTower();

        }
        public void RemoveTower()
           {
               _session.MapHandler.RemoveTower(this);
               _session.GameTower[_session.GameTower.IndexOf(this)] = null;
               hasChanged = true;
            RemoveTowerAura();
               _session.Players.ForEach(p =>
               {
                   p.User.Send(RequestNames.TowerRemove,GetTowerInfo());
               });
               
           }

       public void KilledMob(MobData mobId)
       { 
           Kills++;
           Owner.Kills++;
           Owner.Money+=mobId.GoldDrop;
           Xp+=mobId.XpDrop;
           Owner.Xp += mobId.XpDrop;
           IsFire = false;
           hasChanged = true;
        }


       public MapTowers GetTowerInfo()
       {
           return new MapTowers
           {
               Id = InstanceId,
               X = X,
               Y = Y,
               TowerId = TowerId
           };
       }

       public TowerStateModel ToTowerStateModel()
       {
           hasChanged = false;
           LastUpdate = new TimeSpan(DateTime.UtcNow.Ticks).TotalMilliseconds;
            return new TowerStateModel
           {
               Id = InstanceId,
               TargetId = Targets.Select(tt => tt.InstanceId).ToArray(),
               TowerAura = Auren.Select(x => x.Type).ToArray()
           };
       }
   }
}
