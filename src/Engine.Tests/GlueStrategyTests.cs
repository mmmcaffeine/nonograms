namespace Dgt.Nonograms.Engine.Tests;

public class GlueStrategyTests
{
    public static TheoryData<int[], bool?[], bool?[]> MatchingHintExecuteTheoryData => new()
    {
        // Glued on the far right, and with nothing else to worry about
        { new[] { 3 }, new bool?[] { true, null, null, null, null }, new bool?[] { true, true, true, null, null } },

        // Glued on the far left, and with nothing else to worry about
        { new[] { 3 }, new bool?[] { null, null, null, null, true }, new bool?[] { null, null, true, true, true } },

        // Glued on the far left, and we should preserve cells on the right
        { new[] { 3 }, new bool?[] { true, null, null, false, null }, new bool?[] { true, true, true, false, null } },

        // Glued on the far right and we should preserve cells to the left
        { new[] { 3 }, new bool?[] { false, false, null, null, true }, new bool?[] { false, false, true, true, true } },

        // Glued on the left, but with some eliminated cells first
        { new[] { 2 }, new bool?[] { false, false, true, null, null }, new bool?[] { false, false, true, true, null } },

        // Glued on the right, but with some eliminated cells first, and cells on the left to preserve
        { new[] { 2 }, new bool?[] { false, null, null, true, false }, new bool?[] { false, null, true, true, false } }
    };

    public static TheoryData<int[], bool?[]> NotMatchingHintExecuteTheoryData => new()
    {
        { new[] { 3 }, new bool?[] { null, null, null, null, null } },
        { new[] { 3 }, new bool?[] { false, null, null, null, null } },
        { new[] { 3 }, new bool?[] { false, null, null, null, false } },
        { new[] { 3 }, new bool?[] { null, null, null, null, false } },
        { new[] { 2 }, new bool?[] { false, null, true, false, null } }
    };

    [Theory]
    [MemberData(nameof(MatchingHintExecuteTheoryData))]
    public void Execute_Should_FillCellsAtEdgeOfGridWhenCellAtEdgeIsFilled(int[] hint, bool?[] cells, bool?[] expected) =>
        new GlueStrategy().Execute(hint, cells).Should().Equal(expected);

    [Theory]
    [MemberData(nameof(NotMatchingHintExecuteTheoryData))]
    public void Execute_Should_ReturnUnalteredCellsWhenCellAtEdgeOfGridIsNotFilled(int[] hint, bool?[] cells) =>
        new GlueStrategy().Execute(hint, cells).Should().Equal(cells);
}
