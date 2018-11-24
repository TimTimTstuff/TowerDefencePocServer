namespace TStuff.Game.TowerDefense3d.Server.Logic
{
    public class Vector2
    {
        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X { get; set; }
        public int Y { get; set; }
        public override bool Equals(object obj)
        {
            var x = (Vector2) obj;
            return X == x.X && Y == x.Y;
        }
    }
}