using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace Dgt.Nonograms.Engine;

public record struct Line : IEnumerable<CellState>
{
    private static readonly Regex ParseRegex = new(@"^[01._\- ]+$");

    private readonly CellState[] _cellStates;

    private Line(IEnumerable<CellState> cellStates)
    {
        _cellStates = cellStates.ToArray();
    }

    public static Line Parse(string s)
    {
        if (s is null) throw new ArgumentNullException(nameof(s));
        if (!ParseRegex.IsMatch(s)) throw CreateInvalidLineStringException(s);

        var cellStates = s.Select(Convert);

        return new Line(cellStates);
    }

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

    private static CellState Convert(char c) => c switch
    {
        '0' => CellState.Eliminated,
        '1' => CellState.Filled,
        _   => CellState.Undetermined
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
