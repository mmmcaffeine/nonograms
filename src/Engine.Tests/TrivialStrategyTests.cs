namespace Dgt.Nonograms.Engine.Tests;

public class TrivialStrategyTests
{
    [Theory]
    [InlineData("5", ".....", "11111")]
    [InlineData("2,2", ".....", "11011")]
    [InlineData("1,1,1", ".....", "10101")]
    [InlineData("3,3", ".......", "1110111")]
    public void Execute_Should_FillOrEliminateAllCellsWhenHintDoesMatch(string hint, string line, string expected)
    {
        // Arrange
        IStrategy sut = new TrivialStrategy();

        // Act
        var actual = sut.Execute(hint, line);

        // Assert
        actual.Should().Equal(Line.Parse(expected));
    }

    [Theory]
    [InlineData("1", "...")]
    [InlineData("3", ".....")]
    [InlineData("1,1", ".....")]
    public void Execute_Should_ReturnUnalteredCellsWhenHintDoesNotMatch(string hint, string line)
    {
        // Arrange
        IStrategy sut = new TrivialStrategy();

        // Act
        var actual = sut.Execute(hint, line);

        // Assert
        actual.Should().Equal(Line.Parse(line));
    }
}
