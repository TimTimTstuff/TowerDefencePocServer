using System;
using System.Collections.Generic;
using System.Linq;
using NetworkCommsDotNet.Connections;
using TStuff.Game.TowerDefense3d.lib.ContractObjects;

namespace TStuff.Game.TowerDefense3d.Server.Managers
{
   public class UserHandler
    {
        private List<User> _users = new List<User>();
        private int _currentId = 10;

        public bool Login(Connection con, Login login)
        {

            //Username exists
            if (_users.Any(a => a.Name == login.Name))
            {
                return false;
            }

            
            //ID exists
            if (GetUserByConnection(con) != null) return false;

            var user = new User{Id = _currentId,Name = login.Name};
            user.SetConnection(con);
            _users.Add(user);
          
            login.Id = _currentId;
            con.SendObject("login",login);
            _currentId++;
            return true;
        }

        public void RemoveUser(Connection connection)
        {
            var user = GetUserByConnection(connection);
            if (user != null)
            {
                TStuffLog.Info("Remove user: "+user.Name);
                _users.Remove(user);
            }
        }

        public User GetUserByConnection(Connection connection)
        {
            return _users.SingleOrDefault(a => a.IsUserByConnection(connection.ConnectionInfo));
        }

        public void AddSystemUser(string name,int id)
        {
            var user = new User {Id = id, Name = name};
            user.SetConnection(null);
           
            _users.Add(user);
        }

        public User GetSystemUser(int userId)
        {
            return _users.Single(u=>u.IsSystemUser && u.Id == userId);
        }
    }
}
