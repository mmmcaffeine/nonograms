using System.Text;

namespace Dgt.Nonograms.Engine;

public sealed class Hint
{
    private readonly uint[] _elements;

    public Hint(IEnumerable<uint> elements)
    {
        if (elements is null) throw new ArgumentNullException(nameof(elements));

        _elements = elements.ToArray();

        if (!_elements.Any()) throw new ArgumentException("Value must contain at least one item.", nameof(elements));
        if (_elements.Any(x => x == 0)) throw CreateContainsZeroesException(_elements, elements, nameof(elements));
    }

    private static Exception CreateContainsZeroesException(uint[] elements, IEnumerable<uint> actualValue, string paramName)
    {
        var messageBuilder = new StringBuilder("Value cannot contain zeroes.");
        var zeroes = elements.Select((x, i) => (Element: x, Index: i))
            .Where(t => t.Element == 0)
            .ToList();

        if(zeroes.Count == 1)
        {
            messageBuilder.Append($" Found zero at index {zeroes[0].Index}");
        }
        else
        {
            messageBuilder.Append(" Found zeroes at indices ");

            for(var i = 0; i < zeroes.Count; i++)
            {
                if (i > 0) messageBuilder.Append(", ");
                if (i == zeroes.Count - 1) messageBuilder.Append("and ");

                messageBuilder.Append(zeroes[i].Index);
            }
        }

        messageBuilder.Append('.');

        return new ArgumentException(messageBuilder.ToString(), paramName)
        {
            Data = { { paramName, actualValue} }
        };
    }
}
