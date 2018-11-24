namespace TStuff.Game.TowerDefense3d.Server.Logic
{
    public class InstanceHelper
    {
        private static int _instanceId = 0;

        public static int GetNewInstanceId()
        {
            _instanceId++;
            return _instanceId;
        }
    }
}