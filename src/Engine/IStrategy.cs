namespace Dgt.Nonograms.Engine;

public interface IStrategy
{
    Line Execute(IEnumerable<int> hint, Line line);
}
