namespace TStuff.Game.TowerDefense3d.Server.Logic
{
    public class Node
    {
        // Change this depending on what the desired size is for each element in the grid
        public static int NodeSize = 32;
        public Node Parent;
        public Vector2 Position;
        public Vector2 Center => new Vector2(Position.X + NodeSize / 2, Position.Y + NodeSize / 2);
        public float DistanceToTarget;
        public float Cost;
        public float F
        {
            get
            {
                if (DistanceToTarget != -1 && Cost != -1)
                    return DistanceToTarget + Cost;
                else
                    return -1;
            }
        }
        public bool Walkable;

        public Node(Vector2 pos, bool walkable)
        {
            Parent = null;
            Position = pos;
            DistanceToTarget = -1;
            Cost = 1;
            Walkable = walkable;
        }
    }
}