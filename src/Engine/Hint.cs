﻿using System.Diagnostics.CodeAnalysis;
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

    private Hint() => _elements = Array.Empty<uint>();

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

    public int Length => _elements.Length;

    public uint this[int index]
    {
        get
        {
            if (index < 0 || index >= Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), index, $"Index was out of range. Must be non-negative and less than the length of the hint ({Length}).");
            }

            return _elements[index];
        }
    }

    public static bool TryParse(string s, [NotNullWhen(true)] out Hint? result)
    {
        var elements = (s ?? string.Empty).Split(',')
            .Select(e => e.Trim())
            .Select(value => uint.TryParse(value, out var result) ? (Value: result, Success: true) : (0, false))
            .ToList();

        bool elementsAreValid = elements.Count > 0 && elements.All(e => e.Success && e.Value > 0);

        // Unsure why the cast is needed. The compiler is smart enough to know Value is a uint when it is returned in the tuple, but
        // not when we do the Select against the tuple 😕
        result = elementsAreValid
            ? new Hint(elements.Select(e => (uint)e.Value))
            : null;

        return result is not null;
    }

    public static Hint Parse(string s)
    {
        if (s is null) throw new ArgumentNullException(nameof(s));

        if(!TryParse(s, out var result))
        {
            throw CreateInvalidHintStringException(s);
        }

        return result;
    }

    private static Exception CreateInvalidHintStringException(string s)
    {
        var messageBuilder = new StringBuilder("The input string was not in a correct format.");

        messageBuilder.Append(" A hint must be a comma delimited list of integers with no zeros.");
        messageBuilder.Append($" Actual value was '{s}'.");

        throw new FormatException(messageBuilder.ToString())
        {
            Data = { { "s", s } }
        };
    }

    public static implicit operator string(Hint hint) => hint is null || hint.Length == 0
        ? string.Empty
        // TODO We should not access private data of the parameter like this. Replace it when we can convert to uint[]
        : string.Join(',', hint._elements);

    public static readonly Hint Empty = new();
}
