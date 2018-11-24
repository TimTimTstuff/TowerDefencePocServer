using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using TStuff.Game.TowerDefense3d.Server.Model;

namespace TStuff.Game.TowerDefense3d.Server.Managers
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private Connection Connection { get; set; }
        public GameSession CurrentSession { get; set; }

        public bool IsSystemUser => Connection == null;

        public bool IsUserByConnection(ConnectionInfo cInfo)
        {
            if (Connection == null) return false;
            return (Connection.ConnectionInfo.NetworkIdentifier.Value == cInfo.NetworkIdentifier.Value);
        }



        public void SetConnection(Connection o)
        {
            Connection = o;
        }

        public void Send(string name, object sendObject)
        {
            if(!IsSystemUser)
                Connection.SendObject(name,sendObject);
        }
    }
}