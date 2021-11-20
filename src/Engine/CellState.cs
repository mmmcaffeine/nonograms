namespace Dgt.Nonograms.Engine;

public readonly record struct CellState
{
    private CellState(string description, bool? boolean, char character)
    {
        Description = description;
        Boolean = boolean;
        Character = character;
    }

    public string Description { get; init; }
    public bool? Boolean { get; init; }
    public char Character { get; init; }

    public static implicit operator bool?(CellState cs) => cs.Boolean;

    public static implicit operator CellState(bool? b) => All.First(cs => b == cs.Boolean);

    public static implicit operator char(CellState cs) => cs.Character;

    public static implicit operator CellState(char c) => All.First(cs => c == cs.Character);

    public static CellState Filled { get; } = new(nameof(Filled), true, '1');
    public static CellState Eliminated { get; } = new(nameof(Eliminated), false, '0');
    public static CellState Undetermined { get; } = new(nameof(Undetermined), null, '.');

    public static readonly IReadOnlyList<CellState> All = new List<CellState>(3) { Filled, Eliminated, Undetermined }.AsReadOnly();
}
