namespace Dgt.Nonograms.Engine.Tests;

public class StrategyBaseTests
{
    private class TestableStrategy : StrategyBase
    {
        protected override bool?[] Execute(uint[] hint, bool?[] cells)
        {
            return cells;
        }
    }

    public static TheoryData<int, uint[], Line> InvalidHintTestData => new()
    {
        { 5, new uint[] { 5 }, Line.Parse("1010") },
        { 7, new uint[] { 3, 3 }, Line.Parse("000") },
        { 6, new uint[] { 1, 2, 1 }, Line.Parse("..") }
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
        var act = () => _ = sut.Execute(Array.Empty<uint>(), new bool?[] { null });

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithParameterName("hint")
            .WithMessage("Value must contain at least one item.*");
    }

    [Fact]
    public void Execute_Should_ThrowWhenLineIsNull()
    {
        // Arrange
        var sut = new TestableStrategy();

        // Act
        var act = () => _ = sut.Execute(new[] { 1u }, null!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("line");
    }

    [Fact]
    public void Execute_Should_ThrowWhenLineIsEmpty()
    {
        // Arrange
        var sut = new TestableStrategy();

        // Act
        var act = () => _ = sut.Execute(new[] { 1u }, Array.Empty<bool?>());

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithParameterName("line")
            .WithMessage("Value must contain at least one item.*");
    }

    [Theory]
    [MemberData(nameof(InvalidHintTestData))]
    public void Execute_Should_Throw_WhenHintIsGreaterThanCells(int minimumCells, uint[] hint, Line line)
    {
        // Arrange
        var sut = new TestableStrategy();

        // Act
        var act = () => _ = sut.Execute(hint, line);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithParameterName("hint")
            .WithMessage("It is not possible to fill in enough cells to fulfil the hint.*")
            .WithMessage($"*The hint would require at least {minimumCells} cells, but there are only {line.Length}.*");
    }
}
