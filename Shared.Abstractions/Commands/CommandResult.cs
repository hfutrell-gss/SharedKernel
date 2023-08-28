using Shared.Abstractions.Kernel;

namespace Shared.Abstractions.Commands;

/// <summary>
/// Marker interface for command result
/// </summary>
public interface ICommandResult {}

/// <summary>
/// The most basic result for a command.
/// This enforces that commands have a
/// response of some kind that gives
/// basic indication to how the command went.
/// </summary>
public sealed record CommandResult : Result, ICommandResult
{
    /// <summary>
    /// Create a failed result
    /// </summary>
    /// <param name="failureReasons"></param>
    private CommandResult(IEnumerable<string> failureReasons) : base(failureReasons)
    {

    }
 
    /// <summary>
    /// Create a failed result when an exception was thrown
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="failureReasons"></param>
    private CommandResult(Exception exception, IEnumerable<string> failureReasons) : base(exception, failureReasons)
    {
    }

    private CommandResult() : base()
    {
    }

    public static CommandResult Success()
    {
        return new CommandResult();
    }
     
    /// <summary>
    /// Fail with the exception thrown and a list of failure reasons
    /// </summary>
    /// <param name="ex"></param>
    /// <param name="failureReason"></param>
    /// <returns></returns>
    public static CommandResult Fail(Exception ex, string failureReason)
    {
        return new CommandResult(ex, new[] { failureReason });
    }
    
    /// <summary>
    /// Fail with the exception thrown and a list of failure reasons
    /// </summary>
    /// <param name="ex"></param>
    /// <param name="failureReasons"></param>
    /// <returns></returns>
    public static CommandResult Fail(Exception ex, IEnumerable<string> failureReasons)
    {
        return new CommandResult(ex, failureReasons);
    }
    
    /// <summary>
    /// Fail with a list of failure reasons
    /// </summary>
    /// <param name="failureReason"></param>
    /// <returns></returns>
    public static CommandResult Fail(string failureReason)
    {
        return new CommandResult(new[] { failureReason });
    }
        
    /// <summary>
    /// Fail with a list of failure reasons
    /// </summary>
    /// <param name="failureReasons"></param>
    /// <returns></returns>
    public static CommandResult Fail(IEnumerable<string> failureReasons)
    {
        return new CommandResult(failureReasons);
    }
        
    /// <summary>
    /// Chains functions on <see cref="CommandResult{TCommandResult}"/>
    /// </summary>
    /// <param name="f"></param>
    /// <returns><see cref="CommandResult"/></returns>
    public CommandResult Bind(Func<CommandResult> f)
    {
        if (WasSuccessful)
        {
            try
            {
                return f();
            }
            catch (Exception ex)
            {
                return Fail(new[] {ex.Message, ex.InnerException?.Message ?? "no inner exception"});
            }
        }
            
        return Fail(FailureReasons!);
    }
         
    /// <summary>
    /// Chains functions on <see cref="CommandResult{TCommandResult}"/>
    /// </summary>
    /// <param name="f"></param>
    /// <returns><see cref="CommandResult"/></returns>
    public CommandResult<TCommandResult> Bind<TCommandResult>(Func<CommandResult<TCommandResult>> f)
    {
        if (WasSuccessful)
        {
            try
            {
                return f();
            }
            catch (Exception ex)
            {
                return CommandResult<TCommandResult>.Fail(new[] {ex.Message, ex.InnerException?.Message ?? "no inner exception"});
            }
        }
                
        return CommandResult<TCommandResult>.Fail(FailureReasons!);
    }
        
    /// <summary>
    /// Implicitly convert a result to a <see cref="Task{CommandResult}"/>
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public static implicit operator Task<CommandResult>(CommandResult result) => Task.FromResult(result);
};

/// <summary>
/// A result that carries a value.
/// </summary>
/// <typeparam name="TResult">The captured value to be transmitted</typeparam>
public sealed record CommandResult<TResult> : Result<TResult>, ICommandResult
{
    private CommandResult(TResult resultValue) : base(resultValue)
    {
    }
 
    private CommandResult(IEnumerable<string> failureReasons) : base(failureReasons)
    {
    }
     
    private CommandResult(Exception exception, IEnumerable<string> failureReasons) : base(exception, failureReasons)
    {
    }
 
    /// <summary>
    /// A successful operation with its result
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public static CommandResult<TResult> Success(TResult result)
    {
        return new CommandResult<TResult>(result);
    }
     
    /// <summary>
    /// Fail with a list of failure reasons
    /// </summary>
    /// <param name="failureReason"></param>
    /// <returns></returns>
    public static CommandResult<TResult> Fail(string failureReason)
    {
        return new CommandResult<TResult>(new [] {failureReason});
    }
     
    /// <summary>
    /// Fail with a list of failure reasons
    /// </summary>
    /// <param name="failureReasons"></param>
    /// <returns></returns>
    public static CommandResult<TResult> Fail(IEnumerable<string> failureReasons)
    {
        return new CommandResult<TResult>(failureReasons);
    }
     
    /// <summary>
    /// Chains functions on <see cref="CommandResult{TResult}"/>
    /// </summary>
    /// <param name="f"></param>
    /// <typeparam name="TMapped"></typeparam>
    /// <returns><see cref="CommandResult{TResult}"/></returns>
    public CommandResult<TMapped> Bind<TMapped>(Func<TResult, CommandResult<TMapped>> f)
    {
        if (WasSuccessful)
        {
            try
            {
                return f(ResultValue!);
            }
            catch (Exception ex)
            {
                return new CommandResult<TMapped>(new[] {ex.Message, ex.InnerException?.Message ?? "no inner exception"});
            }
        }
     
        return new CommandResult<TMapped>(FailureReasons!);
    }
         
    /// <summary>
    /// Chains functions on <see cref="CommandResult{TResult}"/>
    /// </summary>
    /// <param name="f"></param>
    /// <typeparam name="TMapped"></typeparam>
    /// <returns><see cref="CommandResult{TResult}"/></returns>
    public CommandResult<TMapped> Map<TMapped>(Func<TResult, TMapped> f)
    {
        if (WasSuccessful)
        {
            try
            {
                return new CommandResult<TMapped>(f(ResultValue!));
            }
            catch (Exception ex)
            {
                return new CommandResult<TMapped>(ex, new [] {ex.Message, ex.InnerException?.Message ?? "no inner exception" });
            }
        }
         
        return new CommandResult<TMapped>(FailureReasons!);
    }

    public static implicit operator CommandResult<TResult>(TResult result) => new(result);
}
