using System.Collections.Generic;

namespace TStuff.Game.TowerDefense3d.Server.Logic.AStar
{
    internal class ComparePfNodeMatrix : IComparer<int>
    {
        readonly PathFinderNodeFast[] _matrix;

        public ComparePfNodeMatrix(PathFinderNodeFast[] matrix)
        {
            _matrix = matrix;
        }

        public int Compare(int a, int b)
        {
            if (_matrix[a].F > _matrix[b].F)
            {
                return 1;
            }

            if (_matrix[a].F < _matrix[b].F)
            {
                return -1;
            }
            return 0;
        }
    }
}