namespace Dgt.Nonograms.Engine.Tests;

public class NoSmallGapsStrategyTests
{
    public static TheoryData<int[], bool?[], bool?[]> TheoryData => new()
    {
        // Hint of 1 means we can't eliminate any gaps because any gap could be the filled cell
        { new[] { 1 }, new bool?[] { false, null, false, null, null, false }, new bool?[] { false, null, false, null, null, false } },

        // The 5 would allow us to eliminate some cells, but the 1 could go anywhere so we're still stuffed
        { new[] { 1, 5 }, new bool?[] { false, null, false, null, null, null, false, null }, new bool?[] { false, null, false, null, null, null, false, null } },

        // We can eliminate the two gaps of two because the 3 cannot go there. In this case we know the 3 has to go
        // on the far right, but that is not our responsibility!
        { new[] { 3 }, new bool?[] { null, null, false, null, null, false, null, null, null }, new bool?[] { false, false, false, false, false, false, null, null, null } },
    };

    [Theory]
    [MemberData(nameof(TheoryData))]
    public void Execute_Should_EliminateAnyGapsSmallerThanSmallestHint(int[] hint, bool?[] cells, bool?[] expected) =>
        new NoSmallGapsStrategy().Execute(hint, cells).Should().Equal(expected);
}
