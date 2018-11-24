using System;
using System.Collections.Generic;
using System.Linq;
using TStuff.Game.TowerDefense3d.lib.ContractObjects;
using TStuff.Game.TowerDefense3d.lib.Enums;
using TStuff.Game.TowerDefense3d.Server.Model;

namespace TStuff.Game.TowerDefense3d.Server.Logic
{
    public class MapHelper
    {
        private GameSession _session;

        public MapHelper(GameSession gameSession)
        {
            _session = gameSession;
        }

        public List<List<Tuple<MapTile, MapTile, MapTile>>> GetCompleteList()
        {
            return  new List<List<Tuple<MapTile, MapTile, MapTile>>>
            {
                BuildSpawnListByTeam1(),
                BuildSpawnListByTeam2(),
                BuildSpawnListByTeam3(),
                BuildSpawnListByTeam4()
            };
        }

        private List<Tuple<MapTile, MapTile, MapTile>> BuildSpawnListByTeam1()
        {
            var spawnList = new List<Tuple<MapTile, MapTile, MapTile>>();
            var spawnTile = _session.MapData.Data.Single(a => a.Type == TileType.Path1Team1Start);
            var wayTile = _session.MapData.Data.SingleOrDefault(a => a.Type == TileType.Path1Team1Way);
            var endTile = _session.MapData.Data.Single(a => a.Type == TileType.Path1Team1end);
            spawnList.Add(new Tuple<MapTile, MapTile, MapTile>(spawnTile, wayTile, endTile));
            try
            {
                var spawnTileoMapTile = _session.MapData.Data.Single(a => a.Type == TileType.Path2Team1Start);
                var wayTileo = _session.MapData.Data.SingleOrDefault(a => a.Type == TileType.Path2Team1Way);
                var endTileo = _session.MapData.Data.Single(a => a.Type == TileType.Path2Team1end);
                spawnList.Add(new Tuple<MapTile, MapTile, MapTile>(spawnTileoMapTile, wayTileo, endTileo));
            }
            catch
            {
                TStuffLog.Debug("Cant load Path"); }
            try
            {
                var spawnTileoMapTile = _session.MapData.Data.Single(a => a.Type == TileType.Path3Team1Start);
                var wayTileo = _session.MapData.Data.SingleOrDefault(a => a.Type == TileType.Path3Team1Way);
                var endTileo = _session.MapData.Data.Single(a => a.Type == TileType.Path3Team1end);
                spawnList.Add(new Tuple<MapTile, MapTile, MapTile>(spawnTileoMapTile, wayTileo, endTileo));
            }
            catch { TStuffLog.Debug("Cant load Path"); }
            try
            {
                var spawnTileoMapTile = _session.MapData.Data.Single(a => a.Type == TileType.Path4Team1Start);
                var wayTileo = _session.MapData.Data.SingleOrDefault(a => a.Type == TileType.Path4Team1Way);
                var endTileo = _session.MapData.Data.Single(a => a.Type == TileType.Path4Team1end);
                spawnList.Add(new Tuple<MapTile, MapTile, MapTile>(spawnTileoMapTile, wayTileo, endTileo));
            }
            catch { TStuffLog.Debug("Cant load Path"); }


            return spawnList;
        }

        private List<Tuple<MapTile, MapTile, MapTile>> BuildSpawnListByTeam2()
        {
            var spawnList = new List<Tuple<MapTile, MapTile, MapTile>>();
            var spawnTile = _session.MapData.Data.Single(a => a.Type == TileType.Path1Team2Start);
            var wayTile = _session.MapData.Data.SingleOrDefault(a => a.Type == TileType.Path1Team2Way);
            var endTile = _session.MapData.Data.Single(a => a.Type == TileType.Path1Team2end);
            spawnList.Add(new Tuple<MapTile, MapTile, MapTile>(spawnTile, wayTile, endTile));
            try
            {
                var spawnTileoMapTile = _session.MapData.Data.Single(a => a.Type == TileType.Path2Team2Start);
                var wayTileo = _session.MapData.Data.SingleOrDefault(a => a.Type == TileType.Path2Team2Way);
                var endTileo = _session.MapData.Data.Single(a => a.Type == TileType.Path2Team2end);
                spawnList.Add(new Tuple<MapTile, MapTile, MapTile>(spawnTileoMapTile, wayTileo, endTileo));
            }
            catch(Exception ex) { TStuffLog.Debug("Cant load Team 2 Path 2: "+ex.Message); }
            try
            {
                var spawnTileoMapTile = _session.MapData.Data.Single(a => a.Type == TileType.Path3Team2Start);
                var wayTileo = _session.MapData.Data.SingleOrDefault(a => a.Type == TileType.Path3Team2Way);
                var endTileo = _session.MapData.Data.Single(a => a.Type == TileType.Path3Team2end);
                spawnList.Add(new Tuple<MapTile, MapTile, MapTile>(spawnTileoMapTile, wayTileo, endTileo));
            }
            catch (Exception ex) { TStuffLog.Debug("Cant load Team 2 Path 3: " + ex.Message); }
            try
            {
                var spawnTileoMapTile = _session.MapData.Data.Single(a => a.Type == TileType.Path4Team2Start);
                var wayTileo = _session.MapData.Data.SingleOrDefault(a => a.Type == TileType.Path4Team2Way);
                var endTileo = _session.MapData.Data.Single(a => a.Type == TileType.Path4Team2end);
                spawnList.Add(new Tuple<MapTile, MapTile, MapTile>(spawnTileoMapTile, wayTileo, endTileo));
            }
            catch (Exception ex) { TStuffLog.Debug("Cant load Team 2 Path 4: " + ex.Message); }


            return spawnList;
        }

        private List<Tuple<MapTile, MapTile, MapTile>> BuildSpawnListByTeam3()
        {var spawnList = new List<Tuple<MapTile, MapTile, MapTile>>();
            try
            {
                
                var spawnTile = _session.MapData.Data.Single(a => a.Type == TileType.Path1Team3Start);
                var wayTile = _session.MapData.Data.SingleOrDefault(a => a.Type == TileType.Path1Team3Way);
                var endTile = _session.MapData.Data.Single(a => a.Type == TileType.Path1Team3end);
                spawnList.Add(new Tuple<MapTile, MapTile, MapTile>(spawnTile, wayTile, endTile));
            }
            catch  { TStuffLog.Debug("Cant load Path"); }
            try
            {
                var spawnTileoMapTile = _session.MapData.Data.Single(a => a.Type == TileType.Path2Team3Start);
                var wayTileo = _session.MapData.Data.SingleOrDefault(a => a.Type == TileType.Path2Team3Way);
                var endTileo = _session.MapData.Data.Single(a => a.Type == TileType.Path2Team3end);
                spawnList.Add(new Tuple<MapTile, MapTile, MapTile>(spawnTileoMapTile, wayTileo, endTileo));
            }
            catch { TStuffLog.Debug("Cant load Path"); }
            try
            {
                var spawnTileoMapTile = _session.MapData.Data.Single(a => a.Type == TileType.Path3Team3Start);
                var wayTileo = _session.MapData.Data.SingleOrDefault(a => a.Type == TileType.Path3Team3Way);
                var endTileo = _session.MapData.Data.Single(a => a.Type == TileType.Path3Team3end);
                spawnList.Add(new Tuple<MapTile, MapTile, MapTile>(spawnTileoMapTile, wayTileo, endTileo));
            }
            catch { TStuffLog.Debug("Cant load Path"); }
            try
            {
                var spawnTileoMapTile = _session.MapData.Data.Single(a => a.Type == TileType.Path4Team3Start);
                var wayTileo = _session.MapData.Data.SingleOrDefault(a => a.Type == TileType.Path4Team3Way);
                var endTileo = _session.MapData.Data.Single(a => a.Type == TileType.Path4Team3end);
                spawnList.Add(new Tuple<MapTile, MapTile, MapTile>(spawnTileoMapTile, wayTileo, endTileo));
            }
            catch { TStuffLog.Debug("Cant load Path"); }


            return spawnList;
        }

        private List<Tuple<MapTile, MapTile, MapTile>> BuildSpawnListByTeam4()
        {
            var spawnList = new List<Tuple<MapTile, MapTile, MapTile>>();
            try
            {
                var spawnTile = _session.MapData.Data.Single(a => a.Type == TileType.Path1Team4Start);
                var wayTile = _session.MapData.Data.SingleOrDefault(a => a.Type == TileType.Path1Team4Way);
                var endTile = _session.MapData.Data.Single(a => a.Type == TileType.Path1Team4end);
                spawnList.Add(new Tuple<MapTile, MapTile, MapTile>(spawnTile, wayTile, endTile));
            }
            catch { TStuffLog.Debug("Cant load Path"); }
            try
            {
                var spawnTileoMapTile = _session.MapData.Data.Single(a => a.Type == TileType.Path2Team4Start);
                var wayTileo = _session.MapData.Data.SingleOrDefault(a => a.Type == TileType.Path2Team4Way);
                var endTileo = _session.MapData.Data.Single(a => a.Type == TileType.Path2Team4end);
                spawnList.Add(new Tuple<MapTile, MapTile, MapTile>(spawnTileoMapTile, wayTileo, endTileo));
            }
            catch { TStuffLog.Debug("Cant load Path"); }
            try
            {
                var spawnTileoMapTile = _session.MapData.Data.Single(a => a.Type == TileType.Path3Team4Start);
                var wayTileo = _session.MapData.Data.SingleOrDefault(a => a.Type == TileType.Path3Team4Way);
                var endTileo = _session.MapData.Data.Single(a => a.Type == TileType.Path3Team4end);
                spawnList.Add(new Tuple<MapTile, MapTile, MapTile>(spawnTileoMapTile, wayTileo, endTileo));
            }
            catch { TStuffLog.Debug("Cant load Path"); }
            try
            {
                var spawnTileoMapTile = _session.MapData.Data.Single(a => a.Type == TileType.Path4Team4Start);
                var wayTileo = _session.MapData.Data.SingleOrDefault(a => a.Type == TileType.Path4Team4Way);
                var endTileo = _session.MapData.Data.Single(a => a.Type == TileType.Path4Team4end);
                spawnList.Add(new Tuple<MapTile, MapTile, MapTile>(spawnTileoMapTile, wayTileo, endTileo));
            }
            catch { TStuffLog.Debug("Cant load Path"); }


            return spawnList;
        }
    }
}