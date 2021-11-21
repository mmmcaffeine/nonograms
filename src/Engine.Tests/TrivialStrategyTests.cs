namespace Dgt.Nonograms.Engine.Tests;

public class TrivialStrategyTests
{
    public static TheoryData<Hint, Line, Line> MatchingHintExecuteTheoryData => new()
    {
        { new Hint(new uint[] { 5 }), Line.Parse("....."), Line.Parse("11111") },
        { new Hint(new uint[] { 2, 2 }), Line.Parse("....."), Line.Parse("11011") },
        { new Hint(new uint[] { 1, 1, 1 }), Line.Parse("....."), Line.Parse("10101") },
        { new Hint(new uint[] { 3, 3 }), Line.Parse("......."), Line.Parse("1110111") }
    };

    public static TheoryData<Hint, Line> NotMatchingHintExecuteTheoryData => new()
    {
        { new Hint(new uint[] { 1 }), Line.Parse("...") },
        { new Hint(new uint[] { 3 }), Line.Parse(".....") },
        { new Hint(new uint[] { 1, 1 }), Line.Parse(".....") }
    };

    [Theory]
    [MemberData(nameof(MatchingHintExecuteTheoryData))]
    public void Execute_Should_FillOrEliminateAllCellsWhenHintDoesMatch(Hint hint, Line line, Line expected) =>
        new TrivialStrategy().Execute(hint, line).Should().Equal(expected);

    [Theory]
    [MemberData(nameof(NotMatchingHintExecuteTheoryData))]
    public void Execute_Should_ReturnUnalteredCellsWhenHintDoesNotMatch(Hint hint, Line line) =>
        new TrivialStrategy().Execute(hint, line).Should().Equal(line);
}
