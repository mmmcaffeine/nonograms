namespace Dgt.Nonograms.Engine.Tests;

public class TrivialStrategyTests
{
    public static TheoryData<int[], Line, Line> MatchingHintExecuteTheoryData => new()
    {
        { new[] { 5 }, Line.Parse("....."), Line.Parse("11111") },
        { new[] { 2, 2 }, Line.Parse("....."), Line.Parse("11011") },
        { new[] { 1, 1, 1 }, Line.Parse("....."), Line.Parse("10101") },
        { new[] { 3, 3 }, Line.Parse("......."), Line.Parse("1110111") }
    };

    public static TheoryData<int[], Line> NotMatchingHintExecuteTheoryData => new()
    {
        { new[] { 1 }, Line.Parse("...") },
        { new[] { 3 }, Line.Parse(".....") },
        { new[] { 1, 1 }, Line.Parse(".....") }
    };

    [Theory]
    [MemberData(nameof(MatchingHintExecuteTheoryData))]
    public void Execute_Should_FillOrEliminateAllCellsWhenHintDoesMatch(IEnumerable<int> hint, Line line, Line expected) =>
        new TrivialStrategy().Execute(hint, line).Should().Equal(expected);

    [Theory]
    [MemberData(nameof(NotMatchingHintExecuteTheoryData))]
    public void Execute_Should_ReturnUnalteredCellsWhenHintDoesNotMatch(IEnumerable<int> hint, Line line) =>
        new TrivialStrategy().Execute(hint, line).Should().Equal(line);
}
