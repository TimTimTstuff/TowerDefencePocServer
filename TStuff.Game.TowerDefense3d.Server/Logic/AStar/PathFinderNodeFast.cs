using System.Runtime.InteropServices;

namespace TStuff.Game.TowerDefense3d.Server.Logic.AStar
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct PathFinderNodeFast
    {
        public int F; // f = gone + heuristic
        public int G;
        public ushort PX; // Parent
        public ushort PY;
        public byte Status;
    }
}
