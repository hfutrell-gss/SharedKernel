using Shared.Abstractions.Kernel;
using Xunit;

namespace Shared.Kernel.TestHelpers;

public static class ResultTestExtensions
{
    public static void AssertSuccessful<TS>(this ResultBase<TS> result)
    {
        result.Match(
            s => Assert.True(true),
            f => Assert.False(true, "Expected successful result"));
    }

    public static void AssertFailure<TS>(this ResultBase<TS> result)
    {
        result.Match(
            s => Assert.False(true, "Expected failed result"),
            f => Assert.True(true));
    }

    public static FailureDetails ExpectFailureAndGet<TS>(this ResultBase<TS> result)
    {
        return result.Match(
            success => throw new Exception("Expected failure"),
            fail => fail);
    }

    public static TS ExpectSuccessAndGet<TS>(this ResultBase<TS> result)
    {
        return result.Match(
            success => success,
            fail => throw new Exception("Expected success"));
    }
}