using System;
using System.Collections.Generic;
using System.Linq;

namespace TStuff.Game.TowerDefense3d.Server.Logic
{
    public class Astar
    {
        private readonly List<List<Node>> _grid;
        private int GridRows => _grid[0].Count;

        private int GridCols => _grid.Count;

        public Astar(List<List<Node>> grid)
        {
            _grid = grid;
        }

        public List<Node> FindPath(Vector2 Start, Vector2 End)
        {
            var start = new Node(Start, true);
            var end = new Node(End, true);

            var path = new Stack<Node>();
            var openList = new List<Node>();
            var closedList = new List<Node>();
            var current = start;

            // add start node to Open List
            openList.Add(start);

            while (openList.Count != 0 && !closedList.Exists(x => x.Position.Equals(end.Position)))
            {
                current = openList[0];
                openList.Remove(current);
                closedList.Add(current);
                var adjacencies = GetAdjacentNodes(current);


                foreach (var n in adjacencies)
                {
                    if (closedList.Contains(n) || !n.Walkable) continue;
                    if (openList.Contains(n)) continue;

                    n.Parent = current;
                    n.DistanceToTarget = Math.Abs(n.Position.X - end.Position.X) + Math.Abs(n.Position.Y - end.Position.Y);
                    n.Cost = 1 + n.Parent.Cost;
                    openList.Add(n);
                    openList = openList.OrderBy(node => node.F).ToList<Node>();
                }
            }

            // construct path, if end was not closed return null
            if (!closedList.Exists(x => x.Position.Equals(end.Position)))
            {
                return null;
            }

            // if all good, return path
            var temp = closedList[closedList.IndexOf(current)];
            while ( temp != null && temp.Parent != start )
            {
                path.Push(temp);
                temp = temp.Parent;
            }
            return path.Reverse().ToList();
        }

        private IEnumerable<Node> GetAdjacentNodes(Node n)
        {
            var temp = new List<Node>();

            var row = (int)n.Position.Y;
            var col = (int)n.Position.X;

            if (row + 1 < GridRows)
            {
                temp.Add(_grid[col][row + 1]);
            }
            if (row - 1 >= 0)
            {
                temp.Add(_grid[col][row - 1]);
            }
            if (col - 1 >= 0)
            {
                temp.Add(_grid[col - 1][row]);
            }
            if (col + 1 < GridCols)
            {
                temp.Add(_grid[col + 1][row]);
            }

            return temp;
        }
    }
}