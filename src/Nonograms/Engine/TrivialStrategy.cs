namespace Dgt.Nonograms.Engine;

public class TrivialStrategy : IStrategy
{
    public IEnumerable<bool?> Execute(IEnumerable<int> hint, IEnumerable<bool?> cells)
    {
        var hintArray = hint.ToArray();

        return CanSolve(hintArray, cells)
            ? Solve(hintArray)
            : cells;
    }

    private static bool CanSolve(int[] hint, IEnumerable<bool?> cells) => hint.Sum() + hint.Length - 1 == cells.Count();

    private static IEnumerable<bool?> Solve(int[] hint)
    {
        for (var i = 0; i < hint.Length; i++)
        {
            for (var j = 0; j < hint[i]; j++)
            {
                yield return true;
            }

            if (i < hint.Length - 1)
            {
                yield return false;
            }
        }
    }
}
