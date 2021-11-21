namespace Dgt.Nonograms.Engine;

public abstract class StrategyBase : IStrategy
{
    public Line Execute(Hint hint, Line line)
    {
        _ = hint ?? throw new ArgumentNullException(nameof(hint));
        _ = line ?? throw new ArgumentNullException(nameof(line));

        if (hint.Length == 0) throw new ArgumentException("Value must contain at least one item.", nameof(hint));
        if (line.Length == 0) throw new ArgumentException("Value must contain at least one item.", nameof(line));
        if (hint.MinimumLineLength > line.Length)
        {
            // You wouldn't normally concat strings like this as it is inefficient, but we're not exactly going to be hitting
            // this code a lot so I don't care that much 😉

            var message = "It is not possible to fill in enough cells to fulfil the hint.";
            message += $" The hint would require at least {hint.MinimumLineLength} cells, but there are only {line.Length}.";

            throw new ArgumentException(message, nameof(hint));
        }

        return Execute((uint[])hint, (bool?[])line);
    }

    protected abstract bool?[] Execute(uint[] hint, bool?[] cells);
}
