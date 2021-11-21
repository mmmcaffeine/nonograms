namespace Dgt.Nonograms.Engine.Tests;

public class NoSmallGapsStrategyTests
{
    public static TheoryData<uint[], Line, Line> TheoryData => new()
    {
        // Hint of 1 means we can't eliminate any gaps because any gap could be the filled cell
        { new uint[] { 1 }, Line.Parse("0.0..0"), Line.Parse("0.0..0") },

        // The 5 would allow us to eliminate some cells, but the 1 could go anywhere so we're still stuffed
        { new uint[] { 1, 5 }, Line.Parse("0.0...0."), Line.Parse("0.0...0.") },

        // We can eliminate the two gaps of two because the 3 cannot go there. In this case we know the 3 has to go
        // on the far right, but that is not our responsibility!
        { new uint[] { 3 }, Line.Parse("..0..0..."), Line.Parse("000000...") },
    };

    [Theory]
    [MemberData(nameof(TheoryData))]
    public void Execute_Should_EliminateAnyGapsSmallerThanSmallestHint(uint[] hintElements, Line line, Line expected) =>
        new NoSmallGapsStrategy().Execute(new Hint(hintElements), line).Should().Equal(expected);
}
