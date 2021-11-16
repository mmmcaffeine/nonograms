namespace Dgt.Nonograms.Engine;

public class TrivialStrategy : IStrategy
{
    public IEnumerable<bool?> Execute(IEnumerable<int> hint, IEnumerable<bool?> cells)
    {
        var hintArray = hint.ToArray();

        for(var i = 0; i < hintArray.Length; i++)
        {
            for(var j = 0; j < hintArray[i]; j++)
            {
                yield return true;
            }

            if (i < hintArray.Length - 1)
            {
                yield return false;
            }
        }
    }
}
