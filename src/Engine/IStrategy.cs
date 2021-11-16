namespace Dgt.Nonograms.Engine;

public interface IStrategy
{
    IEnumerable<bool?> Execute(IEnumerable<int> hint, IEnumerable<bool?> cells);
}
