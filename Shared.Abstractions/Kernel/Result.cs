#pragma warning disable CS0108, CS0114

namespace Shared.Abstractions.Kernel;

/// <summary>
/// A basic result type that reports success or invalidation reasons
/// </summary>
public sealed record Result : Result<Unit>
{
    private Result(Unit success) : base(success)
    {
    }
    
    private Result(FailureDetails failure) : base(failure)
    {
    }

    /// <summary>
    /// Create success
    /// </summary>
    /// <returns></returns>
    public static Result Success() => new(new Unit());
    
    /// <summary>
    /// Create failure
    /// </summary>
    /// <param name="failureDetails"></param>
    /// <returns></returns>
    public static Result Fail(params string[] failureDetails) => new(FailureDetails.From(failureDetails));
}

/// <summary>
/// A basic result that returns a value
/// </summary>
/// <typeparam name="TResult"></typeparam>
public record Result<TResult> : ResultBase<TResult>
{
    /// <summary>
    /// Create a new success
    /// </summary>
    /// <param name="success"></param>
    protected Result(TResult success) : base(success)
    {
    }

    /// <summary>
    /// Create a new failure
    /// </summary>
    /// <param name="details"></param>
    protected Result(FailureDetails details) : base(details)
    {
    }

    /// <summary>
    /// Create success
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public static Result<TResult> Success(TResult result) => new(result);
    
    /// <summary>
    /// Create failure from reasons
    /// </summary>
    /// <param name="reasons"></param>
    /// <returns></returns>
    public static Result<TResult> Fail(params string[] reasons) => new(FailureDetails.From(reasons));
    
    /// <summary>
    /// Create failure from details
    /// </summary>
    /// <param name="details"></param>
    /// <returns></returns>
    public static Result<TResult> Fail(FailureDetails details) => new(details);

    /// <summary>
    /// Implicitly cast to a task type
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public static implicit operator Task<Result<TResult>>(Result<TResult> result) => Task.FromResult(result);
}