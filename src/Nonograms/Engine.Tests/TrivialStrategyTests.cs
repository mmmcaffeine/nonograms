namespace Dgt.Nonograms.Engine.Tests
{
    public class TrivialStrategyTests
    {
        // TODO Account for the following scenarios:

        // * hint is null, contains no elements, or contains negative numbers
        // * cells is null, or contains no elements
        // * hint is greater than count of cells
        // * any cell is already set to false (we don't care if any are set to false)
        // * return cells as is if the hint does not match
        // * more complex hints (e.g. {2, 2} or {3, 1}) should still return true (assuming cells has count of 5)

        [Fact]
        public void Execute_Should_FillAllCellsWhenHintIsAppropriate()
        {
            // Arrange
            var sut = new TrivialStrategy();
            var hint = new[] {5};
            var cells = new bool?[5];

            Array.Fill(cells, null);

            // Act
            var actual = sut.Execute(hint, cells);

            // Assert
            actual.Should().Equal(true, true, true, true, true);
        }
    }
}
