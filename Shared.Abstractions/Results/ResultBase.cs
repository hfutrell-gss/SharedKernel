namespace Shared.Abstractions.Results;

/// <summary>
/// A result was in an invalid state
/// </summary>
public class InvalidResultStateException : InvalidOperationException
{
    /// <summary>
    /// Create an invalid result state exception
    /// </summary>
    public InvalidResultStateException() : base("Success evaluated to null. Result is in an invalid state")
    {
        
    }
}

/// <summary>
/// Base result object
/// </summary>
/// <typeparam name="TSuccess"></typeparam>
public abstract record ResultBase<TSuccess>
{
    private readonly TSuccess? _success;
    private readonly FailureDetails? _failure;

    /// <summary>
    /// True if the operation succeeded
    /// </summary>
    public bool Succeeded => _success is not null;
    
    /// <summary>
    /// True if the operation failed
    /// </summary>
    public bool Failed => _failure is not null;

    /// <summary>
    /// Get the success value if succeeded
    /// </summary>
    /// <exception cref="InvalidOperationException">If retrieving success value on a failed operation</exception>
    public TSuccess SuccessValue =>
        _success ?? throw new InvalidOperationException($"Tried to get success value on a failed operation. Failure reason was {FailureDetails.GetMessage()}", FailureDetails.Exception);
    
    /// <summary>
    /// Get the failure details if failed
    /// </summary>
    /// <exception cref="InvalidOperationException">If retrieving failure value on a successful operation</exception>
    public FailureDetails FailureDetails => 
        _failure ?? throw new InvalidOperationException($"Tried to get failure value on successful operation");
    
    /// <summary>
    /// Internal success constructor
    /// </summary>
    /// <param name="success"></param>
    protected internal ResultBase(TSuccess success)
    {
        _success = success;
    }
    
    /// <summary>
    /// Internal failure constructor
    /// </summary>
    /// <param name="failure"></param>
    protected internal ResultBase(FailureDetails failure)
    {
        _failure = failure;
    }

    /// <summary>
    /// Perform an action on success or failure
    /// </summary>
    /// <param name="forSuccess"></param>
    /// <param name="forFailure"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Resolve(
        Action<TSuccess> forSuccess,
        Action<FailureDetails> forFailure
    )
    {
        Do(forSuccess, forFailure);
    }
    
    /// <summary>
    /// Perform an action on success or failure
    /// </summary>
    /// <param name="forSuccess"></param>
    /// <param name="forFailure"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public Task Resolve(
        Func<TSuccess, Task> forSuccess,
        Func<FailureDetails, Task> forFailure
    )
    {
        return DoAsync(forSuccess, forFailure);
    }
     
    /// <summary>
    /// Perform an operation on success or failure
    /// </summary>
    /// <param name="forSuccess"></param>
    /// <param name="forFailure"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public TResult Resolve<TResult>(
        Func<TSuccess, TResult> forSuccess,
        Func<FailureDetails, TResult> forFailure
    )
    {
        return Do(forSuccess, forFailure);
    }

    /// <summary>
    /// Perform an operation on success or failure
    /// </summary>
    /// <param name="forSuccess"></param>
    /// <param name="forFailure"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public Task<TResult> Resolve<TResult>(
        Func<TSuccess, Task<TResult>> forSuccess,
        Func<FailureDetails, Task<TResult>> forFailure
    )
    {
        return DoAsync(forSuccess, forFailure);
    }

    /// <summary>
    /// Internal implementation of mapping
    /// </summary>
    /// <param name="mapping"></param>
    /// <param name="fail"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TMapped"></typeparam>
    /// <returns></returns>
    protected internal TResult MapCore<TResult, TMapped>(
        Func<TSuccess, TResult> mapping, 
        Func<FailureDetails, TResult> fail
    )
    {
        return Do(mapping, fail);
    }
    
    /// <summary>
    /// Internal implementation of mapping
    /// </summary>
    /// <param name="mapping"></param>
    /// <param name="fail"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TMapped"></typeparam>
    /// <returns></returns>
    protected internal Task<TResult> MapCoreAsync<TResult, TMapped>(
        Func<TSuccess, Task<TResult>> mapping, 
        Func<FailureDetails, Task<TResult>> fail
    )
    {
        return DoAsync(mapping, fail);
    }
     
    private void Do(Action<TSuccess> onSuccess, Action<FailureDetails> onFailure)
    {
        // Check failure first in case TSuccess is a value type
        if (_failure is not null)
        {
            onFailure(_failure);
            return;
        }

        if (_success is null) throw new InvalidResultStateException();
        
        onSuccess(_success);
    }
    
    private TResult Do<TResult>(
        Func<TSuccess, TResult> s,
        Func<FailureDetails, TResult> f
    )
    {
        // Check failure first in case TSuccess is a value type
        if (_failure is not null) return f(_failure);

        if (_success is null) throw new InvalidResultStateException();
        
        try
        {
            return s(_success);
        }
        catch(Exception ex)
        {
            return f(FailureDetails.From(ex));
        }
    }
    
    private async Task DoAsync(
        Func<TSuccess, Task> s,
        Func<FailureDetails, Task> f
    )
    {
        // Check failure first in case TSuccess is a value type
        if (_failure is not null)
        {
            await f(_failure).ConfigureAwait(false);
            return;
        }

        if (_success is null) throw new InvalidResultStateException();
        
        try
        {
            await s(_success).ConfigureAwait(false);
        }
        catch(Exception ex)
        {
            await f(FailureDetails.From(ex)).ConfigureAwait(false);
        }
    }
    
    private async Task<TResult> DoAsync<TResult>(
        Func<TSuccess, Task<TResult>> s,
        Func<FailureDetails, Task<TResult>> f
    )
    {
        // Check failure first in case TSuccess is a value type
        if (_failure is not null) return await f(_failure).ConfigureAwait(false);

        if (_success is null) throw new InvalidResultStateException();
        
        try
        {
            return await s(_success).ConfigureAwait(false);
        }
        catch(Exception ex)
        {
            return await f(FailureDetails.From(ex)).ConfigureAwait(false);
        }
    }
}
