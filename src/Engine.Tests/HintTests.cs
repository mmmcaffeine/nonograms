namespace Dgt.Nonograms.Engine.Tests;

public class HintTests
{
    public static TheoryData<string, uint[]> ValidHintStringTestData => new()
    {
        { "1", new[] { 1u } },
        { "1,10", new[] { 1u, 10u } },
        { "5,6,3", new[] { 5u, 6u, 3u } }
    };

    public static TheoryData<string> InvalidHintStringTestData => new()
    {
        { "" },
        { "   " },
        { "one" },
        { "1,three" },
        { "0,1" },
        { "1,0" },
        { "1,,1" },
        { ",1" },
        { "1," }
    };

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

    [Fact]
    public void Parse_Should_ThrowWhenStringIsNull()
    {
        // Arrange, Act
        var act = () => _ = Hint.Parse(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("s");
    }

    [Theory]
    [MemberData(nameof(InvalidHintStringTestData))]
    public void Parse_Should_ThrowWhenStringIsNotValidHintString(string value)
    {
        // Arrange, Act
        var act = () => _ = _ = Hint.Parse(value);

        // Assert
        act.Should().Throw<FormatException>()
            .WithMessage("*Input string was not in a correct format.*")
            .WithMessage("* A hint must be a comma delimited list of integers with no zeros.*")
            .WithMessage($"*Actual value was '{value}'.*")
            .Where(ex => ex.Data.Contains("s") && Equals(ex.Data["s"], value));
    }

    [Theory]
    [MemberData(nameof(ValidHintStringTestData))]
    public void Parse_Should_ParseElementsInOrderFromCommaDelimitedListOfNumbers(string value, uint[] elements)
    {
        // Arrange, Act
        var hint = Hint.Parse(value);

        // Assert
        using (new AssertionScope())
        {
            hint.Length.Should().Be(elements.Length);

            for (var i = 0; i < elements.Length; i++)
            {
                hint[i].Should().Be(elements[i]);
            }
        }
    }

    [Theory]
    [InlineData(null)]
    [MemberData(nameof(InvalidHintStringTestData))]
    public void TryParse_Should_ReturnFalseAndNoLineWhenStringIsNotValidHintString(string value)
    {
        // Arrange, Act
        var parsed = Hint.TryParse(value, out var hint);

        // Assert
        parsed.Should().BeFalse();
        hint.Should().BeNull();
    }

    [Theory]
    [MemberData(nameof(ValidHintStringTestData))]
    public void TryParse_Should_ReturnTrueAndElementsInOrderWhenStringIsValidHintString(string value, uint[] elements)
    {
        // Arrange, Act
        var parsed = Hint.TryParse(value, out var hint);

        // Assert
        using (new AssertionScope())
        {
            parsed.Should().BeTrue();
            hint.Should().NotBeNull();
            hint!.Length.Should().Be(elements.Length);

            for (var i = 0; i < elements.Length; i++)
            {
                hint[i].Should().Be(elements[i]);
            }
        }
    }
}
