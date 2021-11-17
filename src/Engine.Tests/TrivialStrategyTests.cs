namespace Dgt.Nonograms.Engine.Tests;

public class TrivialStrategyTests
{
    public static TheoryData<int[], bool?[], bool?[]> MatchingHintExecuteTheoryData => new()
    {
        { new[] { 5 }, new bool?[] { null, null, null, null, null }, new bool?[] { true, true, true, true, true } },
        { new[] { 2, 2 }, new bool?[] { null, null, null, null, null }, new bool?[] { true, true, false, true, true } },
        { new[] { 1, 1, 1 }, new bool?[] { null, null, null, null, null }, new bool?[] { true, false, true, false, true } },
        { new[] { 3, 3 }, new bool?[] { null, null, null, null, null, null, null }, new bool?[] { true, true, true, false, true, true, true } }
    };

    public static TheoryData<int[], bool?[]> NotMatchingHintExecuteTheoryData => new()
    {
        { new[] { 1 }, new bool?[] { null, null, null } },
        { new[] { 3 }, new bool?[] { null, null, null, null, null } },
        { new[] { 1, 1 }, new bool?[] { null, null, null, null, null } }
    };

    [Theory]
    [MemberData(nameof(MatchingHintExecuteTheoryData))]
    public void Execute_Should_FillOrEliminateAllCellsWhenHintDoesMatch(IEnumerable<int> hint, IEnumerable<bool?> cells, IEnumerable<bool?> expected) =>
        new TrivialStrategy().Execute(hint, cells).Should().Equal(expected);

    [Theory]
    [MemberData(nameof(NotMatchingHintExecuteTheoryData))]
    public void Execute_Should_ReturnUnalteredCellsWhenHintDoesNotMatch(IEnumerable<int> hint, IEnumerable<bool?> cells) =>
        new TrivialStrategy().Execute(hint, cells).Should().Equal(cells);
}
