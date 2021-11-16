namespace Dgt.Nonograms.Engine;

public class TrivialStrategy : IStrategy
{
    public IEnumerable<bool?> Execute(IEnumerable<int> hint, IEnumerable<bool?> cells)
    {
        for (int i = 0; i < cells.Count(); i++)
        {
            yield return true;
        }
    }
}
