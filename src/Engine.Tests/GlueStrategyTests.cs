namespace Dgt.Nonograms.Engine.Tests;

public class GlueStrategyTests
{
    public static TheoryData<uint[], Line, Line> MatchingHintExecuteTheoryData => new()
    {
        // Glued on the far left, and with nothing else to worry about
        { new uint[] { 3 }, Line.Parse("1...."), Line.Parse("1110.") },

        // Glued on the far right, and with nothing else to worry about
        { new uint[] { 3 }, Line.Parse("....1"), Line.Parse(".0111") },

        // Glued on the far left, and we should preserve cells on the right
        { new uint[] { 3 }, Line.Parse("1..0.0"), Line.Parse("1110.0") },

        // Glued on the far right and we should preserve cells to the left
        { new uint[] { 3 }, Line.Parse(".00..1"), Line.Parse(".00111") },

        // Glued on the left, but with some eliminated cells first
        { new uint[] { 2 }, Line.Parse("001..."), Line.Parse("00110.") },

        // Glued on the right, but with some eliminated cells first, and cells on the left to preserve
        { new uint[] { 2 }, Line.Parse(".0..10"), Line.Parse(".00110") }
    };

    public static TheoryData<uint[], Line> NotMatchingHintExecuteTheoryData => new()
    {
        { new uint[] { 3 }, Line.Parse(".....") },
        { new uint[] { 3 }, Line.Parse("0....") },
        { new uint[] { 3 }, Line.Parse("0...0") },
        { new uint[] { 3 }, Line.Parse("....0") },
        { new uint[] { 2 }, Line.Parse("0.10.") }
    };

    [Theory]
    [MemberData(nameof(MatchingHintExecuteTheoryData))]
    public void Execute_Should_FillCellsAtEdgeOfGridWhenCellAtEdgeIsFilled(uint[] hintElements, Line line, Line expected) =>
        new GlueStrategy().Execute(new Hint(hintElements), line).Should().Equal(expected);

    [Theory]
    [MemberData(nameof(NotMatchingHintExecuteTheoryData))]
    public void Execute_Should_ReturnUnalteredCellsWhenCellAtEdgeOfGridIsNotFilled(uint[] hintElements, Line line) =>
        new GlueStrategy().Execute(new Hint(hintElements), line).Should().Equal(line);
}
