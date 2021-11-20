namespace Dgt.Nonograms.Engine.Tests;

public class GlueStrategyTests
{
    public static TheoryData<int[], Line, Line> MatchingHintExecuteTheoryData => new()
    {
        // Glued on the far left, and with nothing else to worry about
        { new[] { 3 }, Line.Parse("1...."), Line.Parse("1110.") },

        // Glued on the far right, and with nothing else to worry about
        { new[] { 3 }, Line.Parse("....1"), Line.Parse(".0111") },

        // Glued on the far left, and we should preserve cells on the right
        { new[] { 3 }, Line.Parse("1..0.0"), Line.Parse("1110.0") },

        // Glued on the far right and we should preserve cells to the left
        { new[] { 3 }, Line.Parse(".00..1"), Line.Parse(".00111") },

        // Glued on the left, but with some eliminated cells first
        { new[] { 2 }, Line.Parse("001..."), Line.Parse("00110.") },

        // Glued on the right, but with some eliminated cells first, and cells on the left to preserve
        { new[] { 2 }, Line.Parse(".0..10"), Line.Parse(".00110") }
    };

    public static TheoryData<int[], Line> NotMatchingHintExecuteTheoryData => new()
    {
        { new[] { 3 }, Line.Parse(".....") },
        { new[] { 3 }, Line.Parse("0....") },
        { new[] { 3 }, Line.Parse("0...0") },
        { new[] { 3 }, Line.Parse("....0") },
        { new[] { 2 }, Line.Parse("0.10.") }
    };

    [Theory]
    [MemberData(nameof(MatchingHintExecuteTheoryData))]
    public void Execute_Should_FillCellsAtEdgeOfGridWhenCellAtEdgeIsFilled(int[] hint, Line line, Line expected) =>
        new GlueStrategy().Execute(hint, line).Should().Equal(expected);

    [Theory]
    [MemberData(nameof(NotMatchingHintExecuteTheoryData))]
    public void Execute_Should_ReturnUnalteredCellsWhenCellAtEdgeOfGridIsNotFilled(int[] hint, Line line) =>
        new GlueStrategy().Execute(hint, line).Should().Equal(line);
}
