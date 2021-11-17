﻿namespace Dgt.Nonograms.Engine;

public class GlueStrategy : IStrategy
{
    private const int NotFound = -1;

    public IEnumerable<bool?> Execute(IEnumerable<int> hint, IEnumerable<bool?> cells)
    {
        var hintArray = hint.ToArray();
        var cellsArray = cells.ToArray();

        var gluedLeft = ExecuteLeftGlue(hintArray[0], cellsArray);
        var gluedRight = ExecuteRightGlue(hintArray[^1], gluedLeft);

        return gluedRight;
    }

    private static bool?[] ExecuteLeftGlue(int hint, bool?[] cells)
    {
        var glued = new bool?[cells.Length];
        var gluedAt = Array.FindIndex(cells, cell => cell == true);

        if (gluedAt == NotFound) return cells;
        if (cells[0..gluedAt].Any(cell => cell is null)) return cells;

        for (var i = 0; i < glued.Length; i++)
        {
            glued[i] = (i - (gluedAt + hint)) switch
            {
                < 0 => i >= gluedAt,
                0   => false,
                > 0 => cells[i]
            };
        }

        return glued;
    }

    private static bool?[] ExecuteRightGlue(int hint, bool?[] cells)
    {
        var glued = new bool?[cells.Length];
        var gluedAt = Array.FindLastIndex(cells, cell => cell == true);

        if (gluedAt == NotFound) return cells;
        if (cells[gluedAt..].Any(cell => cell is null)) return cells;

        for(var i = 0; i < glued.Length; i++)
        {
            glued[i] = (i - (gluedAt - hint)) switch
            {
                < 0 => cells[i],
                0   => false,
                > 0 => i <= gluedAt
            };
        }

        return glued;
    }
}
