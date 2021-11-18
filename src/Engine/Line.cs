using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;

namespace Dgt.Nonograms.Engine;

public record struct Line : IEnumerable<CellState>
{
    private static readonly Regex ParseRegex = new(@"^[01._\- ]+$");

    private readonly CellState[] _cellStates;

    // TODO If this remains public it needs to be validated
    public Line(IEnumerable<CellState> cellStates)
    {
        _cellStates = cellStates.ToArray();
    }

    public static bool TryParse(string s, [NotNullWhen(true)] out Line? result)
    {
        var canParse = s is not null && IsValidLineString(s);

        result = canParse ? DoParse(s!) : (Line?)null;

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

        messageBuilder.Append(" Lines can only consist of '1' (filled), '0' (eliminated), and any of '.', '_', '-', or ' ' (undetermined).");
        messageBuilder.Append($" Actual value was '{s}'.");

        throw new FormatException(messageBuilder.ToString())
        {
            Data = { { "s", s } }
        };
    }

    private static Line DoParse(string s) => new(s.Select(Convert));

    private static CellState Convert(char c) => c switch
    {
        '0' => CellState.Eliminated,
        '1' => CellState.Filled,
        _   => CellState.Undetermined
    };

    public static implicit operator string(Line l) => string.Create(l.Length, l, (span, state) =>
    {
        for(var i = 0; i < state.Length; i++)
        {
            span[i] = ConvertToChar(state[i]);
        }
    });

    private static char ConvertToChar(CellState cellState) => cellState switch
    {
        CellState.Filled => '1',
        CellState.Eliminated=> '0',
        CellState.Undetermined => '.',
        // This only happens if someone adds an element to the enum, and that's never going to happen 😉
        _ => default
    };

    public static implicit operator bool?[](Line l)
    {
        var values = new bool?[l.Length];

        for(var i = 0;i < values.Length;i++)
        {
            values[i] = ConvertToNullableBool(l[i]);
        }

        return values;
    }

    private static bool? ConvertToNullableBool(CellState cellState) => cellState switch
    {
        CellState.Filled => true,
        CellState.Eliminated => false,
        CellState.Undetermined => null,
        // This only happens if someone adds an element to the enum, and that's never going to happen 😉
        _ => default
    };

    // Return an empty line i.e. no cell states. Can't return null because I am a struct. Explicit and throw?
    public static implicit operator Line(bool?[] b) => new (b.Select(Convert));

    private static CellState Convert(bool? b) => b switch
    {
        true => CellState.Filled,
        false => CellState.Eliminated,
        null => CellState.Undetermined,
    };

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

    IEnumerator<CellState> IEnumerable<CellState>.GetEnumerator()
    {
        return _cellStates.AsEnumerable().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _cellStates.GetEnumerator();
    }
}
