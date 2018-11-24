using System;
using System.Collections.Generic;
using System.Drawing;
using TStuff.Game.TowerDefense3d.lib.ContractObjects;
using TStuff.Game.TowerDefense3d.lib.Enums;
using TStuff.Game.TowerDefense3d.Server.Model.Map;

namespace TStuff.Game.TowerDefense3d.Server.Logic
{
   public class MapFromImageParser
    {
        private MapModel _map;
        private Bitmap _mapImage;
        private MapDataModel _mapData;

        public MapFromImageParser(MapModel map)
        {
            _map = map;
            _mapData = new MapDataModel
            {
                Data = null
            };

            _mapImage = new Bitmap(GConfig.D.MapFileFolderPathRelativ + map.FileName);
        }

        public MapDataModel ParseImage()
        {
            ReadMapData();
            return _mapData;
        }

        private void ReadMapData()
        {
            var pList = new List<MapTile>();
            for (int x = 0; x < _map.Cols; x++)
            {
                for (int y = _map.ColorDefinitionRows; y < _map.Rows + _map.ColorDefinitionRows; y++)
                {
                    var color = _mapImage.GetPixel(x, y);
                         pList.Add(GetTile((TileType) RGBtoInt(color.R, color.G, color.B), x, y - _map.ColorDefinitionRows));
                }
            }

            _mapData.Data = pList.ToArray();

        }

        private MapTile GetTile(TileType type, int x, int y)
        {
            var mapTile = new MapTile
            {
                X = x,
                Y = y,
                Type = type,
                
            };

            switch (type)
            {
                case TileType.Block:
                    mapTile.Block = true;
                     break;
                   
                case TileType.NonBuild:
                    break;
                case TileType.BuildTeam1:
                    mapTile.BuildableByTeamId = 0;
                    break;
                case TileType.BuildTeam2:
                    mapTile.BuildableByTeamId = 1;
                    break;
                case TileType.BuildTeam3:
                    mapTile.BuildableByTeamId = 2;
                   
                    break;
                case TileType.BuildTeam4:
                    mapTile.BuildableByTeamId = 3;
                    break;

                case TileType.Path1Team2Start:
                    break;
                case TileType.Path1Team2Way:
                    break;
                case TileType.Path1Team2end:
                    break;
                case TileType.Path2Team2Start:
                    break;
                case TileType.Path2Team2Way:
                    break;
                case TileType.Path2Team2end:
                    break;
                case TileType.Path3Team2Start:
                    break;
                case TileType.Path3Team2Way:
                    break;
                case TileType.Path3Team2end:
                    break;
                case TileType.Path4Team2Start:
                    break;
                case TileType.Path4Team2Way:
                    break;
                case TileType.Path4Team2end:
                    break;
                case TileType.Path1Team3Start:
                    break;
                case TileType.Path1Team3Way:
                    break;
                case TileType.Path1Team3end:
                    break;
                case TileType.Path2Team3Start:
                    break;
                case TileType.Path2Team3Way:
                    break;
                case TileType.Path2Team3end:
                    break;
                case TileType.Path3Team3Start:
                    break;
                case TileType.Path3Team3Way:
                    break;
                case TileType.Path3Team3end:
                    break;
                case TileType.Path4Team3Start:
                    break;
                case TileType.Path4Team3Way:
                    break;
                case TileType.Path4Team3end:
                    break;
                case TileType.Path1Team4Start:
                    break;
                case TileType.Path1Team4Way:
                    break;
                case TileType.Path1Team4end:
                    break;
                case TileType.Path2Team4Start:
                    break;
                case TileType.Path2Team4Way:
                    break;
                case TileType.Path2Team4end:
                    break;
                case TileType.Path3Team4Start:
                    break;
                case TileType.Path3Team4Way:
                    break;
                case TileType.Path3Team4end:
                    break;
                case TileType.Path4Team4Start:
                    break;
                case TileType.Path4Team4Way:
                    break;
                case TileType.Path4Team4end:
                    break;
                case TileType.Path1Team1Start:
                    break;
                case TileType.Path1Team1Way:
                    break;
                case TileType.Path1Team1end:
                    break;
                case TileType.Path2Team1Start:
                    break;
                case TileType.Path2Team1Way:
                    break;
                case TileType.Path2Team1end:
                    break;
                case TileType.Path3Team1Start:
                    break;
                case TileType.Path3Team1Way:
                    break;
                case TileType.Path3Team1end:
                    break;
                case TileType.Path4Team1Start:
                    break;
                case TileType.Path4Team1Way:
                    break;
                case TileType.Path4Team1end:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            return mapTile;
        }

        public Color GetColor(int color)
        {
            return Color.FromArgb((color >> 16) & 0xff, (color >> 8) & 0xff, (color >> 0) & 0xff);
        }

        public static int RGBtoInt(int r, int g, int b)
        {
            return (r << 16) | (g << 8) | (b << 0);
        }

        private void GenerateHeader()
        {
            var mapHeader = new Dictionary<TileType, int>();
            for (int x = 0; x < _map.ColorDefinitionCols; x++)
            {
                for (int y = 0; y < _map.ColorDefinitionRows; y++)
                {
                    var color = _mapImage.GetPixel(x, y);
                    TStuffLog.Debug(color.ToString() + " x: " + x + " y: " + y);
                }
            }
        }
    }
}
