namespace TStuff.Game.TowerDefense3d.Server.Logic.AStar
{
    public enum HeuristicFormula
    {
        // ReSharper disable InconsistentNaming
        Manhattan = 1,
        MaxDXDY = 2,
        DiagonalShortCut = 3,
        Euclidean = 4,
        EuclideanNoSQR = 5,
        Custom1 = 6
        // ReSharper restore InconsistentNaming
    }
}
