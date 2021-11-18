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

    // TODO We should not really be accessing _cellStates like this. Replace this when Line exposes an indexer
    public static implicit operator string(Line l) => string.Create(l._cellStates.Length, l._cellStates, (span, state) =>
    {
        for(var i = 0; i < state.Length; i++)
        {
            span[i] = Convert(state[i]);
        }
    });

    private static char Convert(CellState cellState) => cellState switch
    {
        CellState.Filled => '1',
        CellState.Eliminated=> '0',
        CellState.Undetermined => '.',
        // This only happens if someone adds an element to the enum, and that's never going to happen 😉
        _ => default
    };

    IEnumerator<CellState> IEnumerable<CellState>.GetEnumerator()
    {
        return _cellStates.AsEnumerable().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _cellStates.GetEnumerator();
    }
}
