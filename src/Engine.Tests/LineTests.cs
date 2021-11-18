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
        line!.Value.Should().Equal(expectedCellStates);
    }

    [Fact]
    public void ImplicitConversionToString_Should_FormatAsOneCharacterPerCellState()
    {
        // Arrange
        var line = new Line(new[] { CellState.Filled, CellState.Eliminated, CellState.Undetermined, CellState.Eliminated, CellState.Filled });

        // Act
        string actual = line;

        // Assert
        actual.Should().Be("10.01");
    }

    [Fact]
    public void ImplicitConversionToNullableBoolArray_Should_ReturnOneNullableBoolPerCellState()
    {
        // Arrange
        var line = new Line(new[] { CellState.Filled, CellState.Eliminated, CellState.Undetermined, CellState.Eliminated, CellState.Filled });

        // Act
        bool?[] actual = line;

        // Assert
        actual.Should().Equal(true, false, null, false, true);
    }

    [Fact]
    public void ImplicitConversionFromNullableBoolArray_Should_HaveOneCellStatePerNullalbeBool()
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
