namespace Dgt.Nonograms.Engine.Tests;

public class HintTests
{
    [Fact]
    public void Ctor_Should_ThrowWhenElementsIsNull()
    {
        // Arrange, Act
        var act = () => _ = new Hint(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("elements");
    }

    [Fact]
    public void Ctor_Should_ThrowWhenElementsIsEmpty()
    {
        // Arrange, Act
        var act = () => _ = new Hint(Array.Empty<uint>());

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithParameterName("elements")
            .WithMessage("*Value must contain at least one item.*");
    }

    [Theory]
    [InlineData("zero at index 0", 0u)]
    [InlineData("zeroes at indices 0, and 1", 0u, 0u)]
    [InlineData("zero at index 0", 0u, 1u)]
    [InlineData("zero at index 1", 1u, 0u)]
    [InlineData("zeroes at indices 0, 1, 3, and 4", 0u, 0u, 1u, 0u, 0u)]
    [InlineData("zero at index 2", 1u, 1u, 0u, 1u, 1u)]
    public void Ctor_Should_ThrowWhenElementsContainsZeros(string messageFragment, params uint[] elements)
    {
        // Arrange, Act
        var act = () => _ = new Hint(elements);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithParameterName("elements")
            .WithMessage("*Value cannot contain zeroes.*")
            .WithMessage($"*Found {messageFragment}.*")
            .Where(ex => ex.Data.Contains("elements") && ReferenceEquals(ex.Data["elements"], elements));
    }

    [Theory]
    [InlineData(1u)]
    [InlineData(2u, 3u)]
    [InlineData(4u, 5u, 6u)]
    public void Length_Should_BeLengthOfArray(params uint[] elements) =>
        new Hint(elements).Length.Should().Be(elements.Length);

    [Fact]
    public void Indexer_Should_ReturnElementAtIndex()
    {
        // Arrange
        var hint = new Hint(new uint [] { 1, 3, 2, 6 });

        // Act, Assert
        using (new AssertionScope())
        {
            hint[0].Should().Be(1);
            hint[1].Should().Be(3);
            hint[2].Should().Be(2);
            hint[3].Should().Be(6);
        }
    }

    [Theory]
    [InlineData(int.MinValue)]
    [InlineData(-1)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(int.MaxValue)]
    public void Indexer_Should_ThrowWhenIndexIsNegativeOrGreaterThanLength(int index)
    {
        // Arrange
        var hint = new Hint(new uint[] { 3, 5, 1 });

        // Act
        var act = () => _ = hint[index];

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("index")
            .WithMessage("*Index was out of range. Must be non-negative and less than the length of the hint (3).*")
            .Where(ex => ex.ActualValue != null && Equals(ex.ActualValue, index));
    }
}
