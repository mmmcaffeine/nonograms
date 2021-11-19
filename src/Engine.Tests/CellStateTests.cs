namespace Dgt.Nonograms.Engine.Tests;

public class CellStateTests
{
    public static TheoryData<bool?, char, CellState> CellStateTestData => new()
    {
        { true, '1', CellState.Filled },
        { false, '0', CellState.Eliminated },
        { null, '.', CellState.Undetermined }
    };

    [Theory]
    [MemberData(nameof(CellStateTestData))]
    [SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters", Justification = "Parameter required to match TheoryData")]
    public void ImplicitConversionToChar_Should_ReturnCharacter(bool? _, char character, CellState cellState)
    {
        // Arrange, Act
        char c = cellState;

        // Assert
        c.Should().Be(character);
    }

    [Theory]
    [MemberData(nameof(CellStateTestData))]
    [SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters", Justification = "Parameter required to match TheoryData")]
    public void ImplicitConversionFromChar_Should_ReturnCellState(bool? _, char character, CellState cellState)
    {
        CellState cs = character;

        // Assert
        cs.Should().Be(cellState);
    }

    [Theory]
    [MemberData(nameof(CellStateTestData))]
    [SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters", Justification = "Parameter required to match TheoryData")]
    public void ImplicitConversionToNullableBool_Should_ReturnCharacter(bool? boolean, char _, CellState cellState)
    {
        // Arrange, Act
        bool? b = cellState;

        // Assert
        b.Should().Be(boolean);
    }

    [Theory]
    [MemberData(nameof(CellStateTestData))]
    [SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters", Justification = "Parameter required to match TheoryData")]
    public void ImplicitConversionFromNullableBool_Should_ReturnCellState(bool? boolean, char _, CellState cellState)
    {
        CellState cs = boolean;

        // Assert
        cs.Should().Be(cellState);
    }
}
