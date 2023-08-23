namespace Shared.Abstractions.Commands;

/// <summary>
/// The most basic result for a command.
/// This enforces that commands have a
/// response of some kind that gives
/// basic indication to how the command went.
/// </summary>
public record CommandResult
{
    /// <summary>
    /// The execution status of the result
    /// </summary>
    public ResultStatus Status { get; protected init; }
    
    /// <summary>
    /// Any errors that occured during command execution
    /// </summary>
    public IEnumerable<string>? Errors { get; protected init; }

    /// <summary>
    /// Uses init stuff
    /// </summary>
    protected CommandResult()
    {
        
    }

    private CommandResult(ResultStatus status)
    {
        Status = status;
    }
    
    /// <summary>
    /// Create the result from state
    /// </summary>
    /// <param name="status"></param>
    /// <param name="errors"></param>
    private CommandResult(ResultStatus status, IEnumerable<string> errors)
    {
        Status = status;
        Errors = errors;
    }

    /// <summary>
    /// A command was successful
    /// </summary>
    /// <returns></returns>
    public static CommandResult Success()
    {
        return new CommandResult(ResultStatus.Success);
    }

    /// <summary>
    /// The command failed with an error
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static CommandResult Failure(string error)
    {
        return Failure(new[] { error });
    }
    
    /// <summary>
    /// The command failed with many errors
    /// </summary>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static CommandResult Failure(IEnumerable<string> errors)
    {
        return new CommandResult(ResultStatus.Failed, errors);
    }
}

/// <summary>
/// A result that carries a value.
/// </summary>
/// <typeparam name="TResult">Self referential for type reasons</typeparam>
/// <typeparam name="TValue">The captured value to be transmitted</typeparam>
public abstract record CommandResult<TResult, TValue> : CommandResult
    where TResult : CommandResult<TResult, TValue>, new()
{
    /// <summary>
    /// The resulting value from execution
    /// </summary>
    public TValue? ResultValue { get; private init; }

    /// <summary>
    /// Create a default constructor
    /// </summary>
    protected CommandResult()
    {
    }

    /// <summary>
    /// Command was successful and has the value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static TResult Success(TValue value)
    {
        return new TResult { ResultValue = value, Status = ResultStatus.Success};
    }
    
    /// <summary>
    /// The command failed with an error
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public new static TResult Failure(string error)
    {
        return new TResult { Errors = new[] { error }, Status = ResultStatus.Failed};
    }
        

    /// <summary>
    /// The command failed with many errors
    /// </summary>
    /// <param name="errors"></param>
    /// <returns></returns>
    public new static TResult Failure(IEnumerable<string> errors)
    {
        return new TResult { Errors = errors, Status = ResultStatus.Failed};
    }
}