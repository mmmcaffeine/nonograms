namespace Dgt.Nonograms.Engine;

public class TrivialStrategy : StrategyBase
{
    protected override bool?[] Execute(int[] hint, bool?[] cells)
    {
        return CanSolve(hint, cells)
            ? Solve(hint).ToArray()
            : cells;
    }

    private static bool CanSolve(int[] hint, bool?[] cells) => hint.Sum() + hint.Length - 1 == cells.Length;

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
