namespace Dgt.Nonograms.Engine;

public abstract class StrategyBase : IStrategy
{
    public IEnumerable<bool?> Execute(IEnumerable<int> hint, IEnumerable<bool?> cells)
    {
        var hintArray = hint is not null ? hint.ToArray() : throw new ArgumentNullException(nameof(hint));
        var cellsArray = cells is not null ? cells.ToArray() : throw new ArgumentNullException(nameof (cells));

        if (hintArray.Length == 0) throw new ArgumentException("Value must contain at least one item.", nameof(hint));
        if (cellsArray.Length == 0) throw new ArgumentException("Value must contain at least one item.", nameof(cells));

        var minimumCells = hintArray.Sum() + hintArray.Length - 1;

        if (minimumCells > cellsArray.Length)
        {
            // You wouldn't normally concat strings like this as it is inefficient, but we're not exactly going to be hitting
            // this code a lot so I don't care that much 😉

            var message = "It is not possible to fill in enough cells to fulfil the hint.";
            message += $" The hint would require at least {minimumCells} cells, but there are only {cellsArray.Length}.";

            throw new ArgumentException(message, nameof(hint));
        }

        // TODO Check that what comes back from DoExecute does not conflict with the incoming cells

        return DoExecute(hintArray, cellsArray);
    }

    protected abstract bool?[] DoExecute(int[] hint, bool?[] cells);
}
