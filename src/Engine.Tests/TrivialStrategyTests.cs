namespace Dgt.Nonograms.Engine.Tests;

public class TrivialStrategyTests
{
    public static TheoryData<uint[], Line, Line> MatchingHintExecuteTheoryData => new()
    {
        { new uint[] { 5 }, Line.Parse("....."), Line.Parse("11111") },
        { new uint[] { 2, 2 }, Line.Parse("....."), Line.Parse("11011") },
        { new uint[] { 1, 1, 1 }, Line.Parse("....."), Line.Parse("10101") },
        { new uint[] { 3, 3 }, Line.Parse("......."), Line.Parse("1110111") }
    };

    public static TheoryData<uint[], Line> NotMatchingHintExecuteTheoryData => new()
    {
        { new uint[] { 1 }, Line.Parse("...") },
        { new uint[] { 3 }, Line.Parse(".....") },
        { new uint[] { 1, 1 }, Line.Parse(".....") }
    };

    [Theory]
    [MemberData(nameof(MatchingHintExecuteTheoryData))]
    public void Execute_Should_FillOrEliminateAllCellsWhenHintDoesMatch(IEnumerable<uint> hintElements, Line line, Line expected) =>
        new TrivialStrategy().Execute(new Hint(hintElements), line).Should().Equal(expected);

    [Theory]
    [MemberData(nameof(NotMatchingHintExecuteTheoryData))]
    public void Execute_Should_ReturnUnalteredCellsWhenHintDoesNotMatch(IEnumerable<uint> hintElements, Line line) =>
        new TrivialStrategy().Execute(new Hint(hintElements), line).Should().Equal(line);
}
