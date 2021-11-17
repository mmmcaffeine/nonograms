namespace Dgt.Nonograms.Engine;

public class GlueStrategy : IStrategy
{
    private const int NotFound = -1;

    public IEnumerable<bool?> Execute(IEnumerable<int> hint, IEnumerable<bool?> cells)
    {
        var hintArray = hint.ToArray();
        var cellsArray = cells.ToArray();

        var gluedLeft = ExecuteLeftGlue(hintArray[0], cellsArray);
        var gluedRight  = ExecuteRightGlue(hintArray[^1], gluedLeft);

        return gluedRight;
    }

    private static bool?[] ExecuteLeftGlue(int hint, bool?[] cells)
    {
        var glued = new bool?[cells.Length];
        var gluedAt = Array.FindIndex(cells, cell => cell == true);

        if (gluedAt == NotFound) return cells;
        if (cells[0..gluedAt].Any(cell => cell is null)) return cells;

        Array.Fill(glued, false, 0, gluedAt);
        Array.Fill(glued, true, gluedAt, hint);

        if(gluedAt + hint < cells.Length)
        {
            glued[gluedAt + hint] = false;
        }

        for(var i = gluedAt + hint + 1; i < glued.Length; i++)
        {
            glued[i] = cells[i];
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
            if (i < gluedAt - hint)
            {
                glued[i] = cells[i];
            }
            else if(i == gluedAt - hint)
            {
                glued[i] = false;
            }
            else
            {
                glued[i] = i <= gluedAt;
            }
        }

        return glued;
    }
}
