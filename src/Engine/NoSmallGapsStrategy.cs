namespace Dgt.Nonograms.Engine;

public class NoSmallGapsStrategy : StrategyBase
{
    protected override bool?[] Execute(uint[] hint, bool?[] cells)
    {
        var smallestHint = hint.Min();
        var indicesOfEliminatedCells = FindAllIndices(cells, c => c == false).ToArray();
        var gapsBetweenEliminatedCells = FindGaps(indicesOfEliminatedCells, cells.Length).ToArray();
        var values = new bool?[cells.Length];

        Array.Copy(cells, values, cells.Length);

        foreach(var (index, length) in gapsBetweenEliminatedCells)
        {
            if(length >= smallestHint)
            {
                continue;
            }

            if (values[index..(index + length - 1)].Any(value => value is not null))
            {
                continue;
            }

            for(int i = 0; i <= length; i++)
            {
                values[index + i] = false;
            }
        }

        return values;
    }

    private static IEnumerable<int> FindAllIndices(bool?[] values, Func<bool?, bool> predicate)
    {
        for (var i = 0; i < values.Length; i++)
        {
            if(predicate(values[i]))
            {
                yield return i;
            }
        }
    }

    private static IEnumerable<(int Index, int Length)> FindGaps(int[] indices, int length)
    {
        if(indices[0] > 0)
        {
            yield return (0, indices[0]);
        }

        for(var i = 0; i < indices.Length - 1; i++)
        {
            yield return (indices[i] + 1, indices[i + 1] - indices[i] - 1);
        }

        if (indices[^1] < length - 1)
        {
            yield return (indices[^1] + 1, length - indices[^1] - 1);
        }
    }
}
