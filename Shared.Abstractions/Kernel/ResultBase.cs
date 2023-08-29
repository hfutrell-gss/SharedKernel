namespace Shared.Abstractions.Kernel;

/// <summary>
/// A result was in an invalid state
/// </summary>
public class InvalidResultStateException : InvalidOperationException
{
    /// <summary>
    /// Create an invalid result state exception
    /// </summary>
    public InvalidResultStateException() : base("Result was in an invalid state")
    {
        
    }
}

/// <summary>
/// Base result object
/// </summary>
/// <typeparam name="TSuccess"></typeparam>
/// <typeparam name="FailureDetails"></typeparam>
public abstract record ResultBase<TSuccess>
{
    private readonly TSuccess? _success;
    private readonly FailureDetails? _failure;

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
    /// <param name="onSuccess"></param>
    /// <param name="onFailure"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Match(
        Action<TSuccess> onSuccess,
        Action<FailureDetails> onFailure
    )
    {
        Do(s => onSuccess(s), f => onFailure(f));
    }
    
    /// <summary>
    /// Perform an operation on success or failure
    /// </summary>
    /// <param name="onSuccess"></param>
    /// <param name="onFailure"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public TResult Match<TResult>(
        Func<TSuccess, TResult> onSuccess,
        Func<FailureDetails, TResult> onFailure
    )
    {
        return Do(onSuccess, onFailure);
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
    
    protected TResult FlatMapCore<TResult, TMapped>(
        Func<TSuccess, TMapped> mapping,
        Func<TMapped, TResult> generator,
        Func<FailureDetails, TResult> fail
    )
    {
        return Do(s => generator(mapping(s)), fail);
    }

    private void Do(Action<TSuccess> onSuccess, Action<FailureDetails> onFailure)
    {
        // Check failure first in case TSuccess is a value type
        if (_failure is not null)
        {
            onFailure(_failure);
            return;
        }

        if (_success is not null)
        {
            onSuccess(_success);
            return;
        }
    
        throw new InvalidResultStateException();
    }
    
    private TResult Do<TResult>(
        Func<TSuccess, TResult> s,
        Func<FailureDetails, TResult> f
        )
    {
        // Check failure first in case TSuccess is a value type
        if (_failure is not null) return f(_failure);

        if (_success is not null)
        {
            try
            {
                return s(_success);
            }
            catch(Exception ex)
            {
                return f(FailureDetails.From(ex));
            }
        }
        
        throw new InvalidResultStateException();
    }
}
