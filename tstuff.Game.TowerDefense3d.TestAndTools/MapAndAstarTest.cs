using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using TStuff.Game.TowerDefense3d.lib.ContractObjects;
using TStuff.Game.TowerDefense3d.lib.Enums;
using TStuff.Game.TowerDefense3d.Server.Logic;
using TStuff.Game.TowerDefense3d.Server.Logic.AStar;
using TStuff.Game.TowerDefense3d.Server.Model.Map;
using Xunit;
using Xunit.Abstractions;

namespace tstuff.Game.TowerDefense3d.TestAndTools
{
   public class MapAndAstarTest
    {

        private readonly ITestOutputHelper output;

        public MapAndAstarTest(ITestOutputHelper output)
        {
            this.output = output;
        }


        private List<Tuple<MapTile, MapTile, MapTile>> BuildSpawnListByTeam(MapDataModel MapData)
        {
            var spawnList = new List<Tuple<MapTile, MapTile, MapTile>>();
            var spawnTile = MapData.Data.Single(a => a.Type == TileType.Path1Team1Start);
            var wayTile = MapData.Data.SingleOrDefault(a => a.Type == TileType.Path1Team1Way);
            var endTile = MapData.Data.Single(a => a.Type == TileType.Path1Team1end);
            spawnList.Add(new Tuple<MapTile, MapTile, MapTile>(spawnTile, wayTile, endTile));
            try
            {
                var spawnTileoMapTile = MapData.Data.Single(a => a.Type == TileType.Path2Team1Start);
                var wayTileo = MapData.Data.SingleOrDefault(a => a.Type == TileType.Path2Team1Way);
                var endTileo = MapData.Data.Single(a => a.Type == TileType.Path2Team1end);
                spawnList.Add(new Tuple<MapTile, MapTile, MapTile>(spawnTileoMapTile, wayTileo, endTileo));
            }
            catch { }
            try
            {
                var spawnTileoMapTile = MapData.Data.Single(a => a.Type == TileType.Path3Team1Start);
                var wayTileo = MapData.Data.SingleOrDefault(a => a.Type == TileType.Path3Team1Way);
                var endTileo = MapData.Data.Single(a => a.Type == TileType.Path3Team1end);
                spawnList.Add(new Tuple<MapTile, MapTile, MapTile>(spawnTileoMapTile, wayTileo, endTileo));
            }
            catch { }
            try
            {
                var spawnTileoMapTile = MapData.Data.Single(a => a.Type == TileType.Path4Team1Start);
                var wayTileo = MapData.Data.SingleOrDefault(a => a.Type == TileType.Path4Team1Way);
                var endTileo = MapData.Data.Single(a => a.Type == TileType.Path4Team1end);
                spawnList.Add(new Tuple<MapTile, MapTile, MapTile>(spawnTileoMapTile, wayTileo, endTileo));
            }
            catch { }


            return spawnList;
        }


        [Fact]
        public void NewAStar()
        {
            var mapData = new MapFromImageParser(new MapModel { FileName = "2_map.png", Cols = 64, Rows = 64 }).ParseImage();

            var data = new byte[64, 64];
            mapData.Data.OrderBy(p => p.Y).ThenBy(p => p.X).ToList().ForEach(e =>
            {
               
                var node = new Node(new Vector2(e.X, e.Y), !e.Block && e.TowerId == null);
                data[e.X,e.Y]=(byte) (!e.Block && e.TowerId == null?1:0);
                // Console.WriteLine("Walkable: " + (node.Walkable ? "ja" : "nein"));

            });
            
            

            var pathFinder = new PathFinder(data,new PathFinderOptions{Diagonals = false});
            var pathList = BuildSpawnListByTeam(mapData);
            
            var bm = new Bitmap(64, 64);
            for (int x = 0; x < 64; x++)
            {
                for (int y = 0; y < 64; y++)
                {
                    bm.SetPixel(x, y, data[x,y]==1 ? Color.White : Color.Black);
                }
            }

            pathList.ForEach(pr =>
            {
                var path = pathFinder.FindPath(new Point(pr.Item1.X, pr.Item1.Y), new Point(pr.Item3.X, pr.Item3.Y));
                path.ForEach(p =>
                {
                    bm.SetPixel(p.X,p.Y,Color.Blue);
                    output.WriteLine("X: " + p.X + "Y: " + p.Y);
                });

            });

            


            bm.Save("TESTMAP_new.png", ImageFormat.Png);
        }

        [Fact]
        public void MapAstarTest()
        {
            var mapData = new MapFromImageParser(new MapModel{FileName = "1_map.png",Cols = 55,Rows = 55}).ParseImage();
            var erg = new List<List<Node>>();
          mapData.Data.OrderBy(p => p.Y).ThenBy(p => p.X).ToList().ForEach(e =>
            {
                if (erg.Count - 1 != e.Y)
                {
                    erg.Add(new List<Node>());
                }
                var node = new Node(new Vector2(e.X, e.Y), !e.Block && e.TowerId == null);
                erg[e.Y].Add(node);
               // Console.WriteLine("Walkable: " + (node.Walkable ? "ja" : "nein"));

            });

            var bm = new Bitmap(55,55);
            erg.ForEach(l =>
            {
                l.ForEach(n =>
                {
                    bm.SetPixel(n.Position.X,n.Position.Y,n.Walkable?Color.White:Color.Black);
                });
                
            });

            var astar = new Astar(erg);
            var path = astar.FindPath(new Vector2(21, 26), new Vector2(25, 22));
           
            path.ForEach(pa =>
            {
                 bm.SetPixel(pa.Position.X,pa.Position.Y,Color.Red);
                output.WriteLine("X: "+pa.Position.X+"Y: "+pa.Position.Y);
            });
            
                bm.Save("TESTMAP.png",ImageFormat.Png);
        }
    }
}
