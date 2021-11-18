﻿namespace Dgt.Nonograms.Engine.Tests;

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

    [Fact]
    public void Parse_Should_ThrowWhenStringIsNull()
    {
        // Arrange, Act
        var act = () => _ = Line.Parse(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("s");
    }

    [Theory]
    [InlineData("")]
    [InlineData("NotValidLineString")]
    [InlineData("xxxxx")]
    [InlineData("12345")]
    [InlineData("1001..Nope")]
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
    [InlineData("")]
    [InlineData("NotValidLineString")]
    [InlineData("xxxxx")]
    [InlineData("12345")]
    [InlineData("1001..Nope")]
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
    public void ImplicitConversionToString_ShouldFormatAsOnceCharacterPerCellState()
    {
        // Arrange
        var line = new Line(new[] { CellState.Filled, CellState.Eliminated, CellState.Undetermined, CellState.Eliminated, CellState.Filled });

        // Act
        string actual = line;

        // Assert
        actual.Should().Be("10.01");
    }
}
