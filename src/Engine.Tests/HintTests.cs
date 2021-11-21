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

    public static TheoryData<bool, string, uint[]> EqualityTestData => new()
    {
        { true, "1", new uint[] { 1u } },
        { true, "2,3", new uint[] { 2u, 3u } },
        { true, "4,5,6", new uint[] { 4u, 5u, 6u } },
        { false, "4,5,6", new uint[] { 4u } },
        { false, "2,3", new uint[] { 3u, 2u } },
        { false, "1", new uint[] { 2u, 3u } }
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

    [Fact]
    public void ImplicitConversionToString_Should_ReturnEmptyWhenHintIsNull()
    {
        // Arrange, Act
        string actual = (Hint)null!;

        // Assert
        actual.Should().BeEmpty();
    }

    [Fact]
    public void ImplicitConversionToString_Should_ReturnEmptyWhenHintIsEmpty()
    {
        // Arrange
        string actual = Hint.Empty;

        // Assert
        actual.Should().BeEmpty();
    }

    [Theory]
    [MemberData(nameof(ValidHintStringTestData))]
    public void ImplicitConversionToString_Should_ReturnCommaDelimitedListOfElements(string expected, uint[] elements)
    {
        // Arrange
        var hint = new Hint(elements);
        string actual = hint;

        // Assert
        expected.Should().Be(actual);
    }

    [Fact]
    public void ImplicitConversionToArrayOfUint_Should_ReturnEmptyArrayWhenHintIsNull()
    {
        // Arrange
        Hint hint = null!;

        // Act
        uint[] actual = hint;

        // Assert
        actual.Should().BeEmpty();

    }

    [Theory]
    [InlineData(1u)]
    [InlineData(2u ,3u)]
    [InlineData(4u, 5u, 6u)]
    public void ImplicitConversionToArrayOfUint_Should_ReturnUintPerElementInOrder(params uint[] elements)
    {
        // Arrange
        var hint = new Hint(elements);

        // Act
        uint[] actual = hint;

        // Assert
        using(new AssertionScope())
        {
            actual.Length.Should().Be(hint.Length);

            for(var i = 0; i < hint.Length; i++)
            {
                actual[i].Should().Be(hint[i]);
            }
        }
    }

    [Fact]
    public void ImplicitConversionFromArrayOfUint_Should_ReturnEmptyWhenArrayIsNull()
    {
        // Arrange
        uint[] elements = null!;

        // Act
        Hint hint = elements;

        // Assert
        hint.Length.Should().Be(0);
    }

    [Theory]
    [InlineData(1u)]
    [InlineData(2u, 3u)]
    [InlineData(4u, 5u, 6u)]
    public void ImplicitConversionFromArrayOfUnit_Should_ReturnOneElementPerItemInArrayInOrder(params uint[] elements)
    {
        // Arrange, Act
        Hint hint = elements;

        // Assert
        using(new AssertionScope())
        {
            hint.Length.Should().Be(elements.Length);

            for(var i = 0; i < elements.Length; i++)
            {
                hint[i].Should().Be(elements[i]);
            }

        }
    }

    [Fact]
    public void Equality_Should_BeTrueWhenBothHintsAreNull()
    {
        // Arrange
        Hint a = null!;
        Hint b = null!;

        // Act, Assert
        using (new AssertionScope())
        {
            (a == b).Should().BeTrue();
            (b == a).Should().BeTrue();

            (a != b).Should().BeFalse();
            (b != a).Should().BeFalse();
        }
    }

    [Fact]
    public void Equality_Should_BeFalseWhenExactlyOneHintIsNull()
    {
        // Arrange
        Hint nullHint = null!;
        var notNullHint = Hint.Parse("1");

        // Act, Assert
        using (new AssertionScope())
        {
            (nullHint == notNullHint).Should().BeFalse();
            (notNullHint == nullHint).Should().BeFalse();

            (nullHint != notNullHint).Should().BeTrue();
            (notNullHint != nullHint).Should().BeTrue();
        }
    }

    [Theory]
    [MemberData(nameof(EqualityTestData))]
    public void Equality_Should_BeTrueWhenHintsHaveSameNumberOfElementsInSameOrder(bool equal, string hintString, params uint[] elements)
    {
        // Arrange
        var a = new Hint(elements);
        var b = Hint.Parse(hintString);

        // Act, Assert
        using (new AssertionScope())
        {
            (a == b).Should().Be(equal);
            (b == a).Should().Be(equal);

            (a != b).Should().Be(!equal);
            (b != a).Should().Be(!equal);
        }
    }

    // The cast ensures we get the right overload of Equals; we're testing the version inherited from object, not the version on the
    // IEquatable interface
    [Fact]
    public void Equals_Should_BeFalseWhenObjIsNull() => Hint.Parse("1").Equals((object?)null).Should().BeFalse();

    // The cast ensures we get the right overload of Equals; we're testing the version inherited from object, not the version on the
    // IEquatable interface
    [Theory]
    [MemberData(nameof(EqualityTestData))]
    public void Equals_Should_CompareLengthAndElementsAtSameIndex(bool equal, string hintString, uint[] elements)
    {
        // Arrange
        var a = Hint.Parse(hintString);
        var b = new Hint(elements);

        // Act, Assert
        using (new AssertionScope())
        {
            a.Equals((object)b).Should().Be(equal);
            b.Equals((object)a).Should().Be(equal);
        }
    }

    [Fact]
    public void EquatableEquals_Should_BeFalseWhenOtherIsNull()
    {
        // Arrange
        IEquatable<Hint> equatable = Hint.Parse("1");

        // Act, Assert
        equatable.Equals(null).Should().BeFalse();
    }

    [Theory]
    [MemberData(nameof(EqualityTestData))]
    public void EquatableEquals_Should_CompareLengthAndElementsAtSameIndex(bool equal, string hintString, uint[] elements)
    {
        // Arrange
        IEquatable<Hint> equatable = Hint.Parse(hintString);
        var other = new Hint(elements);

        // Act, Assert
        equatable.Equals(other).Should().Be(equal);
    }

    [Theory]
    [MemberData(nameof(EqualityTestData))]
    public void GetHashCode_Should_ReturnSameHashCodeWhenElementsAreEqual(bool equal, string hintString, params uint[] elements)
    {
        // Arrange
        var a = Hint.Parse(hintString);
        var b = new Hint(elements);

        // Act, Assert
        (a.GetHashCode() == b.GetHashCode()).Should().Be(equal);
    }

    [Fact]
    public void GetEnumerator_Should_ReturnEnumeratorThatEnumeratesElements()
    {
        // Arrange
        var elements = new uint[] { 3, 6, 8, 1, 12 };
        var hint = new Hint(elements);

        // Act, Assert
        var i = 0;

        using (new AssertionScope())
        {
            foreach (var element in hint)
            {
                element.Should().Be(elements[i]);
                i++;
            }
        }

        i.Should().Be(elements.Length);
    }

    [Theory]
    [InlineData(1, 1u)]
    [InlineData(6, 2u, 3u)]
    [InlineData(17, 4u, 5u, 6u)]
    public void MinimumLineLength_Should_BeSumOfElementsWithGapBetweenElements(int expected, params uint[] elements) =>
        new Hint(elements).MinimumLineLength.Should().Be(expected);
}
