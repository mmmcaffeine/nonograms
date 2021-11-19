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

    public static CellState Filled { get; }  = new(true, '1');
    public static CellState Eliminated { get; } = new(false, '0');
    public static CellState Undetermined { get; } = new(null, '.');
}
