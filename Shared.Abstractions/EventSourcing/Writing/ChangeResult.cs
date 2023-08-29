using Shared.Abstractions.Commands;
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

/// <summary>
/// Extensions to add mapping functionality to results
/// </summary>
public static class ResultMappingExtensions
{
    /// <summary>
    /// Map internal value to new result type
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TMapped"></typeparam>
    /// <typeparam name="TSuccess"></typeparam>
    /// <returns></returns>
    public static CommandResult<TMapped> Map<TSuccess, TMapped>(
        this ResultBase<TSuccess> result,
        Func<TSuccess, CommandResult<TMapped>> mapping)
    {
        return result.MapCore<CommandResult<TMapped>, TMapped>(
            mapping,
            CommandResult<TMapped>.Fail);
    }
    
    /// <summary>
    /// Map internal value to new result type
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TMapped"></typeparam>
    /// <typeparam name="TSuccess"></typeparam>
    /// <returns></returns>
    public static ChangeResult<TMapped> Map<TSuccess, TMapped>(
        this ResultBase<TSuccess> result,
        Func<TSuccess, ChangeResult<TMapped>> mapping)
    {
        return result.MapCore<ChangeResult<TMapped>, TMapped>(
            mapping,
            ChangeResult<TMapped>.Fail);
    }

    /// <summary>
    /// Map internal value to new result type
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TMapped"></typeparam>
    /// <typeparam name="TSuccess"></typeparam>
    /// <returns></returns>
    public static Result<TMapped> Map<TSuccess, TMapped>(
        this ResultBase<TSuccess> result,
        Func<TSuccess, Result<TMapped>> mapping)
    {
        return result.MapCore<Result<TMapped>, TMapped>(
            mapping,
            Result<TMapped>.Fail);
    }
          
}
