namespace Dgt.Nonograms.Engine.Tests;

public class GlueStrategyTests
{
    [Theory]
    [InlineData("3", "1....", "1110.")]     // Glued on the far left, and with nothing else to worry about
    [InlineData("3", "....1", ".0111")]     // Glued on the far right, and with nothing else to worry about
    [InlineData("3", "1..0.0", "1110.0")]   // Glued on the far left, and we should preserve cells on the right
    [InlineData("3", ".00..1", ".00111")]   // Glued on the far right and we should preserve cells to the left
    [InlineData("2", "001...", "00110.")]   // Glued on the left, but with some eliminated cells first
    [InlineData("2", ".0..10", ".00110")]   // Glued on the right, but with some eliminated cells first, and cells on the left to preserve
    public void Execute_Should_FillCellsAtEdgeOfGridWhenCellAtEdgeIsFilled(string hint, string line, string expected)
    {
        // Arrange
        IStrategy sut = new GlueStrategy();

        // Act
        var actual = sut.Execute(hint, line);

        // Assert
        actual.Should().Equal(Line.Parse(expected));
    }

    [Theory]
    [InlineData("3", ".....")]
    [InlineData("3", "0....")]
    [InlineData("3", "0...0")]
    [InlineData("3", "....0")]
    [InlineData("2", "0.10.")]
    public void Execute_Should_ReturnUnalteredCellsWhenCellAtEdgeOfGridIsNotFilled(string hint, string line)
    {
        // Arrange
        IStrategy sut = new GlueStrategy();

        // Act
        var actual = sut.Execute(hint, line);

        // Assert
        actual.Should().Equal(Line.Parse(line));
    }
}
