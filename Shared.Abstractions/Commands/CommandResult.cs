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
    protected CommandResult(TResult success) : base(success)
    {
    }

    protected CommandResult(FailureDetails failure) : base(failure)
    {
    }

    public static CommandResult<TResult> Success(TResult result) => new(result);
    
    public static CommandResult<TResult> Fail(params string[] reasons) => new(FailureDetails.From(reasons));
    
    public static CommandResult<TResult> Fail(FailureDetails failureDetails) => new(failureDetails);
    
    /// <summary>
    /// Map internal value to new result type
    /// </summary>
    /// <param name="mapping"></param>
    /// <typeparam name="TMapped"></typeparam>
    /// <returns></returns>
    public Result<TMapped> Map<TMapped>(Func<TResult, Result<TMapped>> mapping)
    {
        return MapCore<Result<TMapped>, TMapped>(
            mapping,
            Result<TMapped>.Fail);
    }
}
