namespace Dgt.Nonograms.Engine.Tests;

public class NoSmallGapsStrategyTests
{
    [Theory]
    [InlineData("1", "0.0..0", "0.0..0")]           // Hint of 1 means we can't eliminate any gaps because any gap could be the filled cell
    [InlineData("1,5", "0.0...0", "0.0...0")]         // The 5 would allow us to eliminate some cells, but the 1 could go anywhere so we're still stuffed
    [InlineData("3", "..0..0...", "000000...")]     // We can eliminate the two gaps of two because the 3 cannot go there. Filling the 3 on the far right is not our responsibility
    public void Execute_Should_EliminateAnyGapsSmallerThanSmallestHint(string hint, string line, string expected)
    {
        // Arrange
        IStrategy sut = new NoSmallGapsStrategy();

        // Act
        var actual = sut.Execute(hint, line);

        // Assert
        actual.Should().Equal(Line.Parse(expected));
    }
}
