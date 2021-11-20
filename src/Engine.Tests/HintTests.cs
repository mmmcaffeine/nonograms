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
}
