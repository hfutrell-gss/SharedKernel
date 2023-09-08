using Shared.Abstractions.Results;
using Xunit;
using Xunit.Sdk;

namespace Shared.Kernel.TestHelpers;

public static class ResultTestExtensions
{
    public static void AssertSuccessful<TS>(this ResultBase<TS> result)
    {
        result.Resolve(
            _ => Assert.NotNull(result.SuccessValue),
            _ => throw new XunitException("Result failed when success was expected"));
    }

    public static void AssertFailure<TS>(this ResultBase<TS> result)
    {
        result.Resolve(
            _ => throw new XunitException("Result succeeded when failure was expected"),
            _ => Assert.NotNull(result.FailureDetails));
    }

    public static FailureDetails ExpectFailureAndGet<TS>(this ResultBase<TS> result)
    {
        return result.Resolve(
            _ => throw new XunitException("Expected failure"),
            fail => fail);
    }

    public static TS ExpectSuccessAndGet<TS>(this ResultBase<TS> result)
    {
        return result.Resolve(
            success => success is not null ? success : throw new XunitException("Result was null"),
            _ => throw new XunitException("Expected success"));
    }
}