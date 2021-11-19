namespace Dgt.Nonograms.Engine;

public abstract class StrategyBase : IStrategy
{
    public Line Execute(IEnumerable<int> hint, Line line)
    {
        var hintArray = hint is not null ? hint.ToArray() : throw new ArgumentNullException(nameof(hint));
        _ = line ?? throw new ArgumentNullException(nameof(line));

        if (hintArray.Length == 0) throw new ArgumentException("Value must contain at least one item.", nameof(hint));
        if (line.Length == 0) throw new ArgumentException("Value must contain at least one item.", nameof(line));

        var minimumCells = hintArray.Sum() + hintArray.Length - 1;

        if (minimumCells > line.Length)
        {
            // You wouldn't normally concat strings like this as it is inefficient, but we're not exactly going to be hitting
            // this code a lot so I don't care that much 😉

            var message = "It is not possible to fill in enough cells to fulfil the hint.";
            message += $" The hint would require at least {minimumCells} cells, but there are only {line.Length}.";

            throw new ArgumentException(message, nameof(hint));
        }

        return Execute(hintArray, (bool?[])line);
    }

    protected abstract bool?[] Execute(int[] hint, bool?[] cells);
}
