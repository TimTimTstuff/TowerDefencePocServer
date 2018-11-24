using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TStuff.Game.TowerDefense3d.lib.ContractObjects;
using TStuff.Game.TowerDefense3d.Server;
using TStuff.Game.TowerDefense3d.Server.Logic;
using TStuff.Game.TowerDefense3d.Server.Managers;
using Xunit;
using Xunit.Abstractions;

namespace tstuff.Game.TowerDefense3d.TestAndTools
{
  public  class MobSimulator
    {

        private readonly ITestOutputHelper output;
        private GlobalRegister _context;

        public MobSimulator(ITestOutputHelper output)
        {
            this.output = output;
            Setup();
        }

        public void Setup()
        {
            TStuffLog.LogActions.Add((message, serializedObject, level, member, filepat, line) =>
            {
                if (TStuffLog.LogLevel > level) return;
               
                output.WriteLine("[{5},{0}/{1}:{2}, Level:{3}] Message: {4}", filepat.Split('\\').Last(), member, line, level.ToString(), message, DateTime.Now.ToString("T"));
               
            });

            _context = new GlobalRegister();
            _context.User.AddSystemUser(GConfig.D.MainBotName + " 0", 0);
            _context.User.AddSystemUser(GConfig.D.MainBotName + " 1", 1);
            _context.User.AddSystemUser(GConfig.D.MainBotName + " 2", 2);
            _context.User.AddSystemUser(GConfig.D.MainBotName + " 3", 3);
            _context.User.AddSystemUser(GConfig.D.MainBotName + " 4", 4);

        }

        [Fact]
        public void SimulateMobs()
        {
            GConfig.D.GameSimulationTime = 10;
            GConfig.D.LogicUpdateEachMillisecond = 1;


            
            
        }

    }
}
