using Shared.Abstractions.Results;
using Xunit;

namespace Shared.Kernel.TestHelpers;

public static class ResultTestExtensions
{
    public static void AssertSuccessful<TS>(this ResultBase<TS> result)
    {
        result.Resolve(
            _ => Assert.True(true),
            _ => Assert.False(true, "Expected successful result"));
    }

    public static void AssertFailure<TS>(this ResultBase<TS> result)
    {
        result.Resolve(
            _ => Assert.False(true, "Expected failed result"),
            _ => Assert.True(true));
    }

    public static FailureDetails ExpectFailureAndGet<TS>(this ResultBase<TS> result)
    {
        return result.Resolve(
            _ => throw new Exception("Expected failure"),
            fail => fail);
    }

    public static TS ExpectSuccessAndGet<TS>(this ResultBase<TS> result)
    {
        return result.Resolve(
            success => success,
            _ => throw new Exception("Expected success"));
    }
}