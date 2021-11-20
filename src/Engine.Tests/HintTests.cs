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

    // xUnit can't resolve the parameters to the method properly if we type them as uint 😢 We have to type them as int, then convert
    // ourselves to consume the constructor
    [Theory]
    [InlineData("zero at index 0", 0)]
    [InlineData("zeroes at indices 0, and 1", 0, 0)]
    [InlineData("zero at index 0", 0, 1)]
    [InlineData("zero at index 1", 1, 0)]
    [InlineData("zeroes at indices 0, 1, 3, and 4", 0, 0, 1, 0, 0)]
    [InlineData("zero at index 2", 1, 1, 0, 1, 1)]
    public void Ctor_Should_ThrowWhenElementsContainsZeros(string messageFragment, params int[] elements)
    {
        // Arrange, Act
        var uintElements = elements.Cast<uint>();
        var act = () => _ = new Hint(uintElements);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithParameterName("elements")
            .WithMessage("*Value cannot contain zeroes.*")
            .WithMessage($"*Found {messageFragment}.*")
            .Where(ex => ex.Data.Contains("elements") && ReferenceEquals(ex.Data["elements"], uintElements));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2, 3)]
    [InlineData(4, 5, 6)]
    public void Length_Should_BeLengthOfArray(params int[] elements) =>
        new Hint(elements.Cast<uint>()).Length.Should().Be(elements.Length);

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
