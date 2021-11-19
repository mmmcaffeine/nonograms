namespace Dgt.Nonograms.Engine.Tests;

public class LineTests
{
    public static TheoryData<string, CellState[]> ValidLineStringsTestData => new()
    {
        { "1", new[] { CellState.Filled } },
        { "00", new[] { CellState.Eliminated, CellState.Eliminated } },
        { "...", new[] { CellState.Undetermined, CellState.Undetermined, CellState.Undetermined } },
        { "1.001..", new[] { CellState.Filled, CellState.Undetermined, CellState.Eliminated, CellState.Eliminated, CellState.Filled, CellState.Undetermined, CellState.Undetermined } },
        { "._- ", new[] { CellState.Undetermined, CellState.Undetermined, CellState.Undetermined, CellState.Undetermined } }
    };

    public static TheoryData<string?> InvalidLineStringsTestData => new()
    {
        { string.Empty },
        { "NotValidLineString" },
        { "xxxxx" },
        { "12345" },
        { "1001..Nope" },
    };

    public static TheoryData<string, CellState[]> EqualValuesTestData => new()
    {
        { ".", new[] { CellState.Undetermined } },
        { ".0", new[] { CellState.Undetermined, CellState.Eliminated } },
        { "101", new[] { CellState.Filled, CellState.Eliminated, CellState.Filled } },
        { "..1", new[] { CellState.Undetermined, CellState.Undetermined, CellState.Filled } },
        { "0..", new[] { CellState.Eliminated, CellState.Undetermined, CellState.Undetermined } },
    };

    public static TheoryData<string, CellState[]> NotEqualValuesTestData => new()
    {
        { ".", new[] { CellState.Filled } },
        { ".0", new[] { CellState.Eliminated, CellState.Undetermined } },
        { "101", new[] { CellState.Eliminated, CellState.Eliminated, CellState.Filled } },
        { "..1", new[] { CellState.Undetermined, CellState.Filled, CellState.Filled } },
        { "0..", new[] { CellState.Undetermined, CellState.Filled, CellState.Eliminated } },
    };

    [Fact]
    public void Parse_Should_ThrowWhenStringIsNull()
    {
        // Arrange, Act
        var act = () => _ = Line.Parse(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("s");
    }

    [Theory]
    [MemberData(nameof(InvalidLineStringsTestData))]
    public void Parse_Should_ThrowWhenStringIsNotValidLineString(string value)
    {
        // Arrange, Act
        var act = () => _ = _ = Line.Parse(value);

        // Assert
        act.Should().Throw<FormatException>()
            .WithMessage("*Input string was not in a correct format.*")
            .WithMessage("*Lines can only consist of '1' (filled), '0' (eliminated), and any of '.', '_', '-', or ' ' (undetermined).*")
            .WithMessage($"*Actual value was '{value}'.*")
            .Where(ex => ex.Data.Contains("s") && Equals(ex.Data["s"], value));
    }

    [Theory]
    [MemberData(nameof(ValidLineStringsTestData))]
    public void Parse_Should_ReturnLineWithCellStates(string value, CellState[] expectedCellStates) =>
        Line.Parse(value).Should().Equal(expectedCellStates);

    [Theory]
    [InlineData(null)]
    [MemberData(nameof(InvalidLineStringsTestData))]
    public void TryParse_Should_ReturnFalseAndNoLineWhenStringIsNotValidLineString(string value)
    {
        // Arrange, Act
        var parsed = Line.TryParse(value, out var line);

        // Assert
        parsed.Should().BeFalse();
        line.Should().BeNull();

    }

    [Theory]
    [MemberData(nameof(ValidLineStringsTestData))]
    public void TryParse_Should_ReturnTrueAndLineWhenStringIsValidLineString(string value, CellState[] expectedCellStates)
    {
        // Arrange, Act
        var parsed = Line.TryParse(value, out var line);

        // Assert
        parsed.Should().BeTrue();
        line.Should().NotBeNull();
        line.Should().Equal(expectedCellStates);
    }

    [Fact]
    public void ImplicitConversionToString_Should_ReturnEmptyWhenLineIsNull()
    {
        // Arrange, Act
        string actual = (Line)null!;

        // Assert
        actual.Should().BeEmpty();
    }

    [Fact]
    public void ImplicitConversionToString_Should_FormatAsOneCharacterPerCellState()
    {
        // Arrange
        var line = new Line(new[] { CellState.Filled, CellState.Eliminated, CellState.Undetermined, CellState.Eliminated, CellState.Filled });

        // Act
        string? actual = line;

        // Assert
        actual.Should().Be("10.01");
    }

    [Fact]
    public void ImplicitConversionToArrayOfNullableBool_Should_ReturnEmptyArrayWhenLineIsNull()
    {
        // Arrange, Act
        bool?[] actual = (Line)null!;

        // Assert
        actual.Should().BeEmpty();
    }

    [Fact]
    public void ImplicitConversionToArrayOfNullableBool_Should_ReturnOneNullableBoolPerCellState()
    {
        // Arrange
        var line = new Line(new[] { CellState.Filled, CellState.Eliminated, CellState.Undetermined, CellState.Eliminated, CellState.Filled });

        // Act
        bool?[] actual = line;

        // Assert
        actual.Should().Equal(true, false, null, false, true);
    }

    [Fact]
    public void ImplicitConversionFromArrayOfNullableBool_Should_ReturnEmptyLineWhenArrayIsNull()
    {
        // Arrange, Act
        Line line = (bool?[])null!;

        // Assert
        line.Should().BeEmpty();
    }

    [Fact]
    public void ImplicitConversionFromArrayOfNullableBool_Should_HaveOneCellStatePerNullalbeBool()
    {
        // Arrange
        var nullableBools = new bool?[] { true, false, null, false, true };

        // Act
        Line line = nullableBools;

        // Assert
        line.Should().Equal(CellState.Filled, CellState.Eliminated, CellState.Undetermined, CellState.Eliminated, CellState.Filled);
    }

    [Theory]
    [MemberData(nameof(EqualValuesTestData))]
    public void EqualityWithString_Should_BeTrueWhenAllharactersMatchCellStateAtSameIndex(string lineString, CellState[] cellStates)
    {
        // Arrange
        var line = new Line(cellStates);

        // Act, Assert
        using(new AssertionScope())
        {
            (line == lineString).Should().BeTrue();
            (line != lineString).Should().BeFalse();

            (lineString == line).Should().BeTrue();
            (lineString != line).Should().BeFalse();
        }
    }

    [Theory]
    [MemberData(nameof(NotEqualValuesTestData))]
    public void EqualityWithString_Should_BeFalseWhenAnyCharacterDoesNotMatchCellStateAtSameIndex(string lineString, CellState[] cellStates)
    {
        // Arrange
        var line = new Line(cellStates);

        // Act, Assert
        using (new AssertionScope())
        {
            (line == lineString).Should().BeFalse();
            (line != lineString).Should().BeTrue();

            (lineString == line).Should().BeFalse();
            (lineString != line).Should().BeTrue();
        }
    }

    [Fact]
    public void EquatableEquals_Should_BeFalseWhenOtherIsNull()
    {
        // Arrange
        IEquatable<Line> equatable = new Line(new[] { CellState.Filled });

        // Act, Assert
        equatable.Equals(null).Should().BeFalse();
    }

    [Theory]
    [MemberData(nameof(EqualValuesTestData))]
    public void EquatableEquals_Should_BeTrueWhenCharactersMatchCellStateAtSameIndex(string lineString, CellState[] cellStates)
    {
        // Arrange
        var line = Line.Parse(lineString);
        IEquatable<Line> equatable = new Line(cellStates);
        
        // Act, Assert
        equatable.Equals(line).Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(NotEqualValuesTestData))]
    public void EquatableEquals_Should_BeFalseWhenAnyCharacterDoesNotMatchCellStateAtSameIndex(string lineString, CellState[] cellStates)
    {
        // Arrange
        var line = Line.Parse(lineString);
        IEquatable<Line> equatable = new Line(cellStates);

        // Act, Assert
        equatable.Equals(line).Should().BeFalse();
    }

    // The cast ensures we get the right overload of Equals; we're testing the version inherited from object, not the version on the
    // IEquatable interface
    [Fact]
    public void Equals_Should_BeFalseWhenObjIsNull() => new Line(new[] { CellState.Filled }).Equals((object?)null).Should().BeFalse();

    // The cast ensures we get the right overload of Equals; we're testing the version inherited from object, not the version on the
    // IEquatable interface
    [Theory]
    [MemberData(nameof(EqualValuesTestData))]
    public void Equals_Should_BeTrueWhenAllCharactersMatchCellStateAtSameIndex(string lineString, CellState[] cellStates)
    {
        // Arrange
        var a = Line.Parse(lineString);
        var b = new Line(cellStates);

        // Act, Assert
        a.Equals((object)b).Should().BeTrue();
        b.Equals((object)a).Should().BeTrue();
    }

    // The cast ensures we get the right overload of Equals; we're testing the version inherited from object, not the version on the
    // IEquatable interface
    [Theory]
    [MemberData(nameof(NotEqualValuesTestData))]
    public void Equals_Should_BeFalseWhenAnyCharacterDoesNotMatchCellStateAtSameIndex(string lineString, CellState[] cellStates)
    {
        // Arrange
        var a = Line.Parse(lineString);
        var b = new Line(cellStates);

        // Act, Assert
        a.Equals((object)b).Should().BeFalse();
        b.Equals((object)a).Should().BeFalse();
    }

    [Theory]
    [InlineData(".")]
    [InlineData("0")]
    [InlineData("1")]
    [InlineData("10.01")]
    public void GetHashCode_Should_ReturnHasCodeOfStringRepresentation(string line) => Line.Parse(line).GetHashCode().Should().Be(line.GetHashCode());

    [Theory]
    [InlineData(".")]
    [InlineData("111000")]
    [InlineData("1010101010")]
    public void Length_Should_BeLengthOfArray(string value) => Line.Parse(value).Length.Should().Be(value.Length);

    [Fact]
    public void Indexer_Should_ReturnCellStateAtIndex()
    {
        // Arrange
        var line = new Line(new[] {CellState.Filled, CellState.Eliminated, CellState.Undetermined, CellState.Filled});

        // Act, Assert
        using (new AssertionScope())
        {
            line[0].Should().Be(CellState.Filled);
            line[1].Should().Be(CellState.Eliminated);
            line[2].Should().Be(CellState.Undetermined);
            line[3].Should().Be(CellState.Filled);
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
        var line = new Line(new[] { CellState.Filled, CellState.Eliminated, CellState.Undetermined, CellState.Filled });

        // Act
        var act = () => _ = line[index];

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("index")
            .WithMessage("*Index was out of range. Must be non-negative and less than the length of the line (4).*")
            .Where(ex => ex.ActualValue != null && Equals(ex.ActualValue,index));

    }
}
