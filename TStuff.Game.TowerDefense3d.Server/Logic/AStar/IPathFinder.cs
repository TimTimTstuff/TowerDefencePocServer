using System.Collections.Generic;
using System.Drawing;

namespace TStuff.Game.TowerDefense3d.Server.Logic.AStar
{
    public interface IPathFinder
    {
        List<PathFinderNode> FindPath(Point start, Point end);
    }
}
