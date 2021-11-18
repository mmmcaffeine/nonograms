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
}
