namespace Dgt.Nonograms.Engine.Tests;

public class StrategyInterfaceTests
{
    // FakeItEasy does not pick up default interface implementations. We can define a type that does pick those methods up,
    // but can still be faked. This gives us the best of both worlds in that we get the default behaviour, but can still
    // assert against the other methods
    public abstract class TestableStrategy : IStrategy
    {
        public abstract Line Execute(Hint hint, Line line);
    }

    [Fact]
    public void ExecuteWithStrings_Should_ThrowWhenHintStringIsNotValid()
    {
        // Arrange
        IStrategy fakeStrategy = A.Fake<TestableStrategy>();

        // Act
        var act = () => _ = fakeStrategy.Execute("I Am Not A Hint", ".....");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Value could not be parsed into a hint.*")
            .WithMessage("*Actual value was 'I Am Not A Hint'.*")
            .WithParameterName("hint")
            .Where(ex => ex.Data.Contains("hint") && ex.Data["hint"]!.Equals("I Am Not A Hint"))
            .WithInnerException<FormatException>();
            
    }

    [Fact]
    public void ExecuteWithStrings_Should_ThrowWhenLineStringIsNotValid()
    {
        // Arrange
        IStrategy fakeStrategy = A.Fake<TestableStrategy>();

        // Act
        var act = () => _ = fakeStrategy.Execute("2,2", "I Am Not A Line");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Value could not be parsed into a line.*")
            .WithMessage("*Actual value was 'I Am Not A Line'.*")
            .WithParameterName("line")
            .Where(ex => ex.Data.Contains("line") && ex.Data["line"]!.Equals("I Am Not A Line"))
            .WithInnerException<FormatException>();
    }

    [Theory]
    [InlineData("1", "000")]
    [InlineData("2,2", ".....")]
    [InlineData("3,1", "000...0.1")]
    public void ExecuteWithStrings_Should_ExecuteWithParsedHintAndLine(string hint, string line)
    {
        // Arrange
        IStrategy fakeStrategy = A.Fake<TestableStrategy>();
        var expectedHint = Hint.Parse(hint);
        var expectedLine = Line.Parse(line);

        // Act
        _ = fakeStrategy.Execute(hint, line);

        // Assert
        A.CallTo(() => fakeStrategy.Execute(expectedHint, expectedLine)).MustHaveHappenedOnceExactly();
    }
}
