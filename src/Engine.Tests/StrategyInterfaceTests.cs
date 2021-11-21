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
