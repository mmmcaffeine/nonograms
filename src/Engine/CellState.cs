namespace Dgt.Nonograms.Engine;

public readonly record struct CellState
{
    private CellState(bool? boolean, char character)
    {
        Boolean = boolean;
        Character = character;
    }

    public bool? Boolean { get; init; }
    public char Character { get; init; }

    public static implicit operator bool?(CellState cs) => cs.Boolean;

    public static implicit operator CellState(bool? b) => CellStates.First(cs => b == cs.Boolean);

    public static implicit operator char(CellState cs) => cs.Character;

    public static implicit operator CellState(char c) => CellStates.First(cs => c == cs.Character);

    public static CellState Filled { get; }  = new(true, '1');
    public static CellState Eliminated { get; } = new(false, '0');
    public static CellState Undetermined { get; } = new(null, '.');

    private static readonly List<CellState> CellStates = new() { Filled, Eliminated, Undetermined};}
