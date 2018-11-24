namespace TStuff.Game.TowerDefense3d.Server.Model.Map
{
   public class MapModel
   {
       public string Name { get; set; }
       public int Id { get; set; }
       public int Rows { get; set; }
       public int Cols { get; set; }
       public int Teams { get; set; }
       public int MaxPlayers { get; set; }
       
       public int ColorDefinitionCols = 21;
       public int ColorDefinitionRows = 3;
       public MapColorDefinition ColorDefinition { get; set; }
      
       public string FileName { get; set; }
       public string Description { get; set; }
   }

}
