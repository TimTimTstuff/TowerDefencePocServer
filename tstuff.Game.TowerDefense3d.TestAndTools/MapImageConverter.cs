using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Newtonsoft.Json;
using TStuff.Game.TowerDefense3d.Server.Model.Map;
using Xunit;

namespace tstuff.Game.TowerDefense3d.TestAndTools
{
   public class MapImageConverter
    {

       
        [Fact]
        public void CreateImage()
        {
            var mapDef = new MapModel()
            {
                Name = "Bot Test Map",
                ColorDefinition = new MapColorDefinition(),
                Cols = 64,
                Rows = 64,
                Id =6,
                Description = "",
                Teams = 4,
                MaxPlayers = 2
            };
            var dc = new MapColorDefinitionDefaultColors();
            var sizeRows = mapDef.ColorDefinitionRows+mapDef.Rows;
            var sizeCols = mapDef.ColorDefinitionCols>mapDef.Cols?mapDef.ColorDefinitionCols:mapDef.Cols;


            var myMap = new Bitmap(sizeCols,sizeRows);
            #region ColorSet
            myMap.SetPixel(0, 0, GetColor(dc.Block));
            myMap.SetPixel(1, 0, GetColor(dc.NonBuild));
            myMap.SetPixel(2, 0, GetColor(dc.BuildTeam1));
            myMap.SetPixel(3, 0, GetColor(dc.BuildTeam2));
            myMap.SetPixel(4, 0, GetColor(dc.BuildTeam3));
            myMap.SetPixel(5, 0, GetColor(dc.BuildTeam4));

            myMap.SetPixel(6, 0, GetColor(dc.Path1Team1Start));
            myMap.SetPixel(6, 1, GetColor(dc.Path1Team1Way));
            myMap.SetPixel(6, 2, GetColor(dc.Path1Team1end));

            myMap.SetPixel(7, 0, GetColor(dc.Path2Team1Start));
            myMap.SetPixel(7, 1, GetColor(dc.Path2Team1Way));
            myMap.SetPixel(7, 2, GetColor(dc.Path2Team1end));

            myMap.SetPixel(8, 0, GetColor(dc.Path3Team1Start));
            myMap.SetPixel(8, 1, GetColor(dc.Path3Team1Way));
            myMap.SetPixel(8, 2, GetColor(dc.Path3Team1end));

            myMap.SetPixel(9, 0, GetColor(dc.Path4Team1Start));
            myMap.SetPixel(9, 1, GetColor(dc.Path4Team1Way));
            myMap.SetPixel(9, 2, GetColor(dc.Path4Team1end));

            myMap.SetPixel(10, 0, GetColor(dc.Path1Team2Start));
            myMap.SetPixel(10, 1, GetColor(dc.Path1Team2Way));
            myMap.SetPixel(10, 2, GetColor(dc.Path1Team2end));

            myMap.SetPixel(11, 0, GetColor(dc.Path2Team2Start));
            myMap.SetPixel(11, 1, GetColor(dc.Path2Team2Way));
            myMap.SetPixel(11, 2, GetColor(dc.Path2Team2end));

            myMap.SetPixel(12, 0, GetColor(dc.Path3Team2Start));
            myMap.SetPixel(12, 1, GetColor(dc.Path3Team2Way));
            myMap.SetPixel(12, 2, GetColor(dc.Path3Team2end));

            myMap.SetPixel(13, 0, GetColor(dc.Path4Team2Start));
            myMap.SetPixel(13, 1, GetColor(dc.Path4Team2Way));
            myMap.SetPixel(13, 2, GetColor(dc.Path4Team2end));

            myMap.SetPixel(14, 0, GetColor(dc.Path1Team3Start));
            myMap.SetPixel(14, 1, GetColor(dc.Path1Team3Way));
            myMap.SetPixel(14, 2, GetColor(dc.Path1Team3end));

            myMap.SetPixel(15, 0, GetColor(dc.Path2Team3Start));
            myMap.SetPixel(15, 1, GetColor(dc.Path2Team3Way));
            myMap.SetPixel(15, 2, GetColor(dc.Path2Team3end));

            myMap.SetPixel(16, 0, GetColor(dc.Path3Team3Start));
            myMap.SetPixel(16, 1, GetColor(dc.Path3Team3Way));
            myMap.SetPixel(16, 2, GetColor(dc.Path3Team3end));

            myMap.SetPixel(17, 0, GetColor(dc.Path4Team3Start));
            myMap.SetPixel(17, 1, GetColor(dc.Path4Team3Way));
            myMap.SetPixel(17, 2, GetColor(dc.Path4Team3end));

            myMap.SetPixel(18, 0, GetColor(dc.Path1Team4Start));
            myMap.SetPixel(18, 1, GetColor(dc.Path1Team4Way));
            myMap.SetPixel(18, 2, GetColor(dc.Path1Team4end));

            myMap.SetPixel(19, 0, GetColor(dc.Path2Team4Start));
            myMap.SetPixel(19, 1, GetColor(dc.Path2Team4Way));
            myMap.SetPixel(19, 2, GetColor(dc.Path2Team4end));

            myMap.SetPixel(20, 0, GetColor(dc.Path3Team4Start));
            myMap.SetPixel(20, 1, GetColor(dc.Path3Team4Way));
            myMap.SetPixel(20, 2, GetColor(dc.Path3Team4end));

            myMap.SetPixel(21, 0, GetColor(dc.Path4Team4Start));
            myMap.SetPixel(21, 1, GetColor(dc.Path4Team4Way));
            myMap.SetPixel(21, 2, GetColor(dc.Path4Team4end));
#endregion

            //BuildBorder
            for (var x = 0; x < mapDef.Rows; x++)
            {
                for (var y = 3; y < mapDef.Cols+3; y++)
                {
                    if((x == 0 || x == mapDef.Rows-1) ||(y == 3 || y == mapDef.Cols-1+3))
                    {
                        myMap.SetPixel(x,y,GetColor(dc.Block));
                    }
                }
            }

            if (!Directory.Exists("exportImage"))
            {
                Directory.CreateDirectory("exportImage");
            }

            mapDef.FileName = "exportImage\\"+ mapDef.Id + "_map.png";
            myMap.Save(mapDef.FileName,ImageFormat.Png);
            File.WriteAllText("exportImage\\" + mapDef.Id + "_map.json", JsonConvert.SerializeObject(mapDef));
        }

        public Color GetColor(int color)
        {
            return Color.FromArgb((color >> 16) & 0xff, (color >> 8) & 0xff, (color >> 0) & 0xff);
        }
    }
}
