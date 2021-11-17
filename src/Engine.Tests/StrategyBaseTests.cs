namespace Dgt.Nonograms.Engine.Tests;

public class StrategyBaseTests
{
    private class TestableStrategy : StrategyBase
    {
        protected override bool?[] Execute(int[] hint, bool?[] cells)
        {
            return cells;
        }
    }

    public static TheoryData<int, int[], bool?[]> InvalidHintTestData => new()
    {
        { 5, new[] { 5 }, new bool?[] { true, false, true, false } },
        { 7, new[] { 3, 3 }, new bool?[] { false, false, false } },
        { 6, new[] { 1, 2, 1 }, new bool?[] { null, null } }
    };

    [Fact]
    public void Execute_Should_ThrowWhenHintIsNull()
    {
        // Arrange
        var sut = new TestableStrategy();

        // Act
        var act = () => _ = sut.Execute(null!, new bool?[] { null });

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("hint");
    }

    [Fact]
    public void Execute_Should_ThrowWhenHintIsEmpty()
    {
        // Arrange
        var sut = new TestableStrategy();

        // Act
        var act = () => _ = sut.Execute(Array.Empty<int>(), new bool?[] { null });

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithParameterName("hint")
            .WithMessage("Value must contain at least one item.*");
    }

    [Fact]
    public void Execute_Should_ThrowWhenCellsIsNull()
    {
        // Arrange
        var sut = new TestableStrategy();

        // Act
        var act = () => _ = sut.Execute(new[] { 1 }, null!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("cells");
    }

    [Fact]
    public void Execute_Should_ThrowWhenCellsIsEmpty()
    {
        // Arrange
        var sut = new TestableStrategy();

        // Act
        var act = () => _ = sut.Execute(new[] { 1 }, Array.Empty<bool?>());

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithParameterName("cells")
            .WithMessage("Value must contain at least one item.*");
    }

    [Theory]
    [MemberData(nameof(InvalidHintTestData))]
    public void Execute_Should_Throw_WhenHintIsGreaterThanCells(int minimumCells, int[] hint, bool?[] cells)
    {
        // Arrange
        var sut = new TestableStrategy();

        // Act
        var act = () => _ = sut.Execute(hint, cells);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithParameterName("hint")
            .WithMessage("It is not possible to fill in enough cells to fulfil the hint.*")
            .WithMessage($"*The hint would require at least {minimumCells} cells, but there are only {cells.Length}.*");
    }
}
