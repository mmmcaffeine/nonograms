using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;

namespace Dgt.Nonograms.Engine;

public sealed class Line : IEnumerable<CellState>, IEquatable<Line>
{
    private static readonly Regex ParseRegex = new(BuildParsePattern());

    private static string BuildParsePattern()
    {
        var characters = string.Create(CellState.All.Count, CellState.All, (span, state) =>
        {
            for (var i = 0; i < span.Length; i++)
            {
                span[i] = state[i];
            }
        });

        return $@"^[{characters}]+$";
    }

    private readonly CellState[] _cellStates;

    public static readonly Line Empty = new(Array.Empty<CellState>());

    public Line(IEnumerable<CellState> cellStates)
    {
        if(cellStates is null) throw new ArgumentNullException(nameof(cellStates));

        _cellStates = cellStates.ToArray();
    }

    public static bool TryParse(string s, [NotNullWhen(true)] out Line? result)
    {
        var canParse = s is not null && IsValidLineString(s);

        result = canParse ? DoParse(s!) : null;

        return result is not null;
    }

    public static Line Parse(string s)
    {
        if (s is null) throw new ArgumentNullException(nameof(s));
        if (!IsValidLineString(s)) throw CreateInvalidLineStringException(s);

        return DoParse(s);
    }

    private static bool IsValidLineString(string s) => ParseRegex.IsMatch(s);

    private static Exception CreateInvalidLineStringException(string s)
    {
        var messageBuilder = new StringBuilder("The input string was not in a correct format.");

        messageBuilder.Append(" Lines can only consist of");

        for(var i = 0; i < CellState.All.Count; i++)
        {
            var isLastItem = i == CellState.All.Count - 1;
            var cellState = CellState.All[i];

            if (isLastItem)
            {
                messageBuilder.Append(" or");
            }

            messageBuilder.Append($" '{cellState.Character}' ({cellState.Description})");
            messageBuilder.Append(isLastItem ? '.' : ',');
        }

        messageBuilder.Append($" Actual value was '{s}'.");

        throw new FormatException(messageBuilder.ToString())
        {
            Data = { { "s", s } }
        };
    }

    private static Line DoParse(string s) => new(s.Select(c => (CellState)c));

    public static implicit operator string(Line l)
    {
        if (l is null)
        {
            return string.Empty;
        }

        return string.Create(l.Length, l, (span, state) =>
        {
            for (var i = 0; i < state.Length; i++)
            {
                span[i] = state[i];
            }
        });
    }

    public static implicit operator bool?[](Line l)
    {
        if(l is null)
        {
            return Array.Empty<bool?>();
        }

        var values = new bool?[l.Length];

        for (var i = 0; i < values.Length; i++)
        {
            values[i] = l[i];
        }

        return values;
    }

    public static implicit operator Line(bool?[] b)
    {
        if(b is null)
        {
            return Empty;
        }

        var cellStates = b.Select(x => (CellState)x);

        return new(cellStates);
    }

    public int Length => _cellStates.Length;

    public CellState this[int index]
    {
        get
        {
            if(index < 0 || index >= Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), index, $"Index was out of range. Must be non-negative and less than the length of the line ({Length}).");
            }

            return _cellStates[index];
        }
    }

    public bool Equals(Line? other)
    {
        if(other is null)
        {
            return false;
        }
        else if(ReferenceEquals(this, other))
        {
            return true;
        }
        else if(other.Length != Length)
        {
            return false;
        }

        // We could cast both to string and compare those, but this avoids the allocations that would occur with that implementation
        for(var i = 0; i < Length; i++)
        {
            if(this[i] != other[i])
            {
                return false;
            }
        }

        return true;
    }

    public override bool Equals(object? obj)
    {
        if(obj is null)
        {
            return false;
        }
        else if(obj is Line l)
        {
            return Equals(l);
        }
        else
        {
            return false;
        }
    }

    public override int GetHashCode() => ((string)this).GetHashCode();

    IEnumerator<CellState> IEnumerable<CellState>.GetEnumerator()
    {
        return _cellStates.AsEnumerable().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _cellStates.GetEnumerator();
    }
}
