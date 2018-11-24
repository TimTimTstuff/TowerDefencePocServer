using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJsonSchema;
using TStuff.Game.TowerDefense3d.lib.ContractObjects;
using Xunit;

namespace tstuff.Game.TowerDefense3d.TestAndTools
{
   public class JsonShema
    {
        

        [Fact]
        public void GenerateJsonScheam()
        {
            var mob =  JsonSchema4.FromTypeAsync<MobHolder>();
            var tower = JsonSchema4.FromTypeAsync<TowerHolder>();
            var rmob = JsonSchema4.FromTypeAsync<MobRaceHolder>();
            var rtower = JsonSchema4.FromTypeAsync<TowerRaceHolder>();
            
            File.WriteAllText("mob.schema.json",mob.Result.ToJson());
            File.WriteAllText("tower.schema.json", tower.Result.ToJson());
            File.WriteAllText("mob_race.schema.json", rmob.Result.ToJson());
            File.WriteAllText("tower_race.schema.json", rtower.Result.ToJson());


        }

    }
}
