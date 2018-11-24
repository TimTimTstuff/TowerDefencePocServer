using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TStuff.Game.TowerDefense3d.lib.ContractObjects;
using TStuff.Game.TowerDefense3d.Server.Logic;
using TStuff.Game.TowerDefense3d.Server.Logic.AStar;
using TStuff.Game.TowerDefense3d.Server.Model.Map;

namespace TStuff.Game.TowerDefense3d.Server.Model.GameObjects
{
   public class ServerMap
    {
        private GameSession _session;

        private List<List<Tuple<MapTile, MapTile, MapTile>>> _spawnList;
        private PathFinder _astar;
        private Random _random;

        public ServerMap(GameSession session)
        {
            _random = new Random();
            _session = session;
            _spawnList = new MapHelper(_session).GetCompleteList();
            UpdateMapData();
        }

        public bool IsReady { get; set; }
        public PathFinder AStar => _astar;

        public void DestroyTower(ServerMob serverMob, int nextBlockX, int nextBlockY)
        {
            _session.GameTower.SingleOrDefault(a => a.X == nextBlockX && a.Y == nextBlockY)?.RemoveTower();
            //TODO: Notify user
        }

        public bool IsTowerOnPosition(int currentGoalX, int currentGoalY)
        {
           return _session.GameTower.Where(a=>a!=null).Any(a => a.X == currentGoalX && a.Y == currentGoalY);
        }

        public bool CanPlaceTower(GameUser user, int cX, int cY)
        {
            if (IsTowerOnPosition(cX, cY)) return false;
            if (!IsBuildable(cX, cY,user.TeamId)){ return false;}
            if (!SimulateAllPossible(cX,cY)){ return false;}

            return true;
        }

        private bool IsBuildable(int x, int y, int teamId)
        {
            var tile = GetTileByCoords(x, y);
            return !tile.Block && tile.TowerId == null && tile.BuildableByTeamId == teamId;
        }

        public MapTile GetTileByCoords(int x, int y)
        {
           return _session.MapData.Data.Single(a => a.X == x && a.Y == y);
        }

        public void PlaceTower(ServerTowerModel tower)
        {
            GetTileByCoords(tower.X,tower.Y).TowerId = tower.TowerId;
            UpdateMapData();
        }

        public void RemoveTower(ServerTowerModel tower)
        {
            GetTileByCoords(tower.X, tower.Y).TowerId = null;
            UpdateMapData();
        }

        public void UpdateMapData()
        {
            var data = new byte[_session.SelectedMap.Cols, _session.SelectedMap.Rows];
            _session.MapData.Data.OrderBy(p => p.Y).ThenBy(p => p.X).ToList().ForEach(e =>
            {
                data[e.X, e.Y] = (byte)(!e.Block && e.TowerId == null ? 1 : 0);
            });
            _astar = new PathFinder(data, new PathFinderOptions { Diagonals = false,SearchLimit = GConfig.D.AStarMaxSearch});
            IsReady = true;
        }

        public Tuple<MapTile, MapTile, MapTile> GetRandomSpawn(int toTeam)
        {
           return _spawnList[toTeam][_random.Next(0, _spawnList[toTeam].Count)];
        }

        public List<PathFinderNode> GetPath(Point start, Point goal)
        {
            //ToList because of copy. Remove it, and all enemys have the same path and everything will be broken!
          return  _astar.FindPath(start, goal)?.ToList();
        }

        public bool AllDefaultPathPossible()
        {
            for (var team = 0; team < _spawnList.Count; team++)
            {
                for (var lane = 0; lane < _spawnList[team].Count; lane++)
                {
                    var elem = _spawnList[team][lane];
                    if (elem.Item2 != null)
                    {
                        if (_astar.FindPath(new Point(elem.Item1.X,elem.Item1.Y), new Point(elem.Item2.X, elem.Item2.Y)) == null)
                        {
                            return false;
                        }
                        if (_astar.FindPath(new Point(elem.Item2.X,elem.Item2.Y), new Point(elem.Item3.X, elem.Item3.Y)) == null)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (_astar.FindPath(new Point(elem.Item1.X, elem.Item1.Y), new Point(elem.Item3.X, elem.Item3.Y)) == null)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public bool SimulateAllPossible(int x, int y)
        {
            var data = new byte[_session.SelectedMap.Cols, _session.SelectedMap.Rows];
            _session.MapData.Data.OrderBy(p => p.Y).ThenBy(p => p.X).ToList().ForEach(e =>
            {
                data[e.X, e.Y] = (byte)(!e.Block && e.TowerId == null ? 1 : 0);
                if (e.X == x && e.Y == y)
                {
                    data[e.X, e.Y] = 0;
                }
            });
           var astar = new PathFinder(data, new PathFinderOptions { Diagonals = false, SearchLimit = 999999 });
            for (var team = 0; team < _spawnList.Count; team++)
            {
                for (var lane = 0; lane < _spawnList[team].Count; lane++)
                {
                    var elem = _spawnList[team][lane];
                    if (elem.Item2 != null)
                    {
                        if (astar.FindPath(new Point(elem.Item1.X, elem.Item1.Y), new Point(elem.Item2.X, elem.Item2.Y)) == null)
                        {
                            return false;
                        }
                        if (astar.FindPath(new Point(elem.Item2.X, elem.Item2.Y), new Point(elem.Item3.X, elem.Item3.Y)) == null)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (astar.FindPath(new Point(elem.Item1.X, elem.Item1.Y), new Point(elem.Item3.X, elem.Item3.Y)) == null)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public List<Tuple<MapTile, MapTile, MapTile>> GetAllSpawns(int teamid)
        {
            return _spawnList[teamid];
        }

        public MapTile GetRandomBuildTile(int forTeam)
        {

            var tile = _session.MapData.Data.ToList().Where(a => a.BuildableByTeamId == forTeam && a.TowerId == null).ToList();
            if (tile.Count <= 0) return null;
            return tile[_random.Next(0, tile.Count)];

        }

     
    }
}
