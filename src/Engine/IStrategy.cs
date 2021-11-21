namespace Dgt.Nonograms.Engine;

public interface IStrategy
{
    Line Execute(Hint hint, Line line);
}
