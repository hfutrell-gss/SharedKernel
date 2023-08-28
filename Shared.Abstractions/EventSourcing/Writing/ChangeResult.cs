using Shared.Abstractions.Kernel;

#pragma warning disable CS0108, CS0114
namespace Shared.Abstractions.EventSourcing.Writing;

/// <summary>
/// The result of a change operation on an aggregate
/// </summary>
/// <typeparam name="TResult"></typeparam>
public sealed record ChangeResult<TResult> : Result<TResult>
{
    private ChangeResult(TResult resultValue) : base(resultValue)
    {
    }

    private ChangeResult(IEnumerable<string> failureReasons) : base(failureReasons)
    {
    }
    
    private ChangeResult(Exception exception, IEnumerable<string> invalidationReasons) : base(exception, invalidationReasons)
    {
    }

    /// <summary>
    /// A successful operation with its result
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public static ChangeResult<TResult> Success(TResult result)
    {
        return new ChangeResult<TResult>(result);
    }
    
    /// <summary>
    /// Fail with a list of failure reasons
    /// </summary>
    /// <param name="invalidationReasons"></param>
    /// <returns></returns>
    public static ChangeResult<TResult> Fail(IEnumerable<string> invalidationReasons)
    {
        return new ChangeResult<TResult>(invalidationReasons);
    }
    
    /// <summary>
    /// Chains functions on <see cref="ChangeResult{TResult}"/>
    /// </summary>
    /// <param name="f"></param>
    /// <typeparam name="TMapped"></typeparam>
    /// <returns><see cref="ChangeResult{TResult}"/></returns>
    public ChangeResult<TMapped> Bind<TMapped>(Func<TResult, ChangeResult<TMapped>> f)
    {
        if (WasSuccessful)
        {
            try
            {
                return f(ResultValue!);
            }
            catch (Exception ex)
            {
                return new ChangeResult<TMapped>(new[] {ex.Message, ex.InnerException?.Message ?? "no inner exception"});
            }
        }
    
        return new ChangeResult<TMapped>(FailureReasons!);
    }
        
    /// <summary>
    /// Chains functions on <see cref="ChangeResult{TResult}"/>
    /// </summary>
    /// <param name="f"></param>
    /// <typeparam name="TMapped"></typeparam>
    /// <returns><see cref="ChangeResult{TResult}"/></returns>
    public ChangeResult<TMapped> Map<TMapped>(Func<TResult, TMapped> f)
    {
        if (WasSuccessful)
        {
            try
            {
                return new ChangeResult<TMapped>(f(ResultValue!));
            }
            catch (Exception ex)
            {
                return new ChangeResult<TMapped>(ex, new [] {ex.Message, ex.InnerException?.Message ?? "no inner exception" });
            }
        }
        
        return new ChangeResult<TMapped>(FailureReasons!);
    }
}
