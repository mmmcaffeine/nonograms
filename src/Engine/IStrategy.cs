namespace Dgt.Nonograms.Engine;

public interface IStrategy
{
    Line Execute(string hint, string line) => Execute(Hint.Parse(hint), Line.Parse(line));
    Line Execute(Hint hint, Line line);
}
