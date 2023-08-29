using Shared.Abstractions.Kernel;

namespace Shared.Abstractions.Commands;

/// <summary>
/// The most basic result for a command.
/// This enforces that commands have a
/// response of some kind that gives
/// basic indication to how the command went.
/// </summary>
public sealed record CommandResult : CommandResult<Unit>
{
    private CommandResult(Unit success) : base(success)
    {
    }

    private CommandResult(FailureDetails failure) : base(failure)
    {
    }
    
    public static CommandResult Success() => new(new Unit());
        
    public static CommandResult Fail(params string[] reasons) => new(FailureDetails.From(reasons));
    
    public static CommandResult Fail(FailureDetails failureDetails) => new(failureDetails);
};

/// <summary>
/// A result that carries a value.
/// </summary>
/// <typeparam name="TResult">The captured value to be transmitted</typeparam>
public record CommandResult<TResult> : ResultBase<TResult>
{
    /// <summary>
    /// Create a successful command result
    /// </summary>
    /// <param name="success"></param>
    protected CommandResult(TResult success) : base(success)
    {
    }

    /// <summary>
    /// Create a failed command result
    /// </summary>
    /// <param name="failure"></param>
    protected CommandResult(FailureDetails failure) : base(failure)
    {
    }

    /// <summary>
    /// Create a successful command result
    /// </summary>
    /// <param name="result"></param>
    public static CommandResult<TResult> Success(TResult result) => new(result);

    /// <summary>
    /// Create a failed command result
    /// </summary>
    /// <param name="reasons"></param>
    public static CommandResult<TResult> Fail(params string[] reasons) => new(FailureDetails.From(reasons));

    /// <summary>
    /// Create a failed command result
    /// </summary>
    /// <param name="failureDetails"></param>
    public static CommandResult<TResult> Fail(FailureDetails failureDetails) => new(failureDetails);
}
