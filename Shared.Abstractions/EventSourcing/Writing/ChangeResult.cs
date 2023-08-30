using Shared.Abstractions.Kernel;

namespace Shared.Abstractions.EventSourcing.Writing;


/// <summary>
/// The result of a change operation on an aggregate
/// </summary>
/// <typeparam name="TResult"></typeparam>
public sealed record ChangeResult<TResult> : ResultBase<TResult>
{
    private ChangeResult(TResult success) : base(success)
    {
    }

    private ChangeResult(FailureDetails failureDetails) : base(failureDetails)
    {
    }

    /// <summary>
    /// Create a successful change result
    /// </summary>
    /// <param name="success"></param>
    /// <returns></returns>
    public static ChangeResult<TResult> Success(TResult success) => new(success);
    
    /// <summary>
    /// Create a failed change result
    /// </summary>
    /// <param name="reasons"></param>
    /// <returns></returns>
    public static ChangeResult<TResult> Fail(params string[] reasons) => new(FailureDetails.From(reasons));
    
    /// <summary>
    /// Create a failed change result
    /// </summary>
    /// <param name="details"></param>
    /// <returns></returns>
    public static ChangeResult<TResult> Fail(FailureDetails details) => new(details);

    /// <summary>
    /// Map internal value and keep result structure
    /// </summary>
    /// <param name="mapping"></param>
    /// <typeparam name="TMapped"></typeparam>
    /// <returns></returns>
    public ChangeResult<TMapped> FlatMap<TMapped>(Func<TResult, TMapped> mapping)
    {
        return FlatMapCore<ChangeResult<TMapped>, TMapped>(
            mapping,
            m => new ChangeResult<TMapped>(m),
            ChangeResult<TMapped>.Fail);
    }
}