using Shared.Abstractions.Commands;
using Shared.Abstractions.Kernel;

namespace Shared.Abstractions.EventSourcing.Writing;

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
    public static Task<CommandResult<TMapped>> Map<TSuccess, TMapped>(
        this ResultBase<TSuccess> result,
        Func<TSuccess, Task<CommandResult<TMapped>>> mapping)
    {
        return result.MapCoreAsync<CommandResult<TMapped>, TMapped>(
            mapping,
            f => Task.FromResult(CommandResult<TMapped>.Fail(f)));
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
    public static Task<ChangeResult<TMapped>> Map<TSuccess, TMapped>(
        this ResultBase<TSuccess> result,
        Func<TSuccess, Task<ChangeResult<TMapped>>> mapping)
    {
        return result.MapCoreAsync<ChangeResult<TMapped>, TMapped>(
            mapping,
            f => Task.FromResult(ChangeResult<TMapped>.Fail(f)));
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
    
    /// <summary>
    /// Map internal value to new result type
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TSuccess"></typeparam>
    /// <returns></returns>
    public static Task<Result> Map<TSuccess>(
        this ResultBase<TSuccess> result,
        Func<TSuccess, Task<Result>> mapping)
    {
        return result.MapCoreAsync<Result, Unit>(
            mapping,
            f => Task.FromResult(Result.Fail(f)));
    }
    
    /// <summary>
    /// Map internal value to new result type
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TSuccess"></typeparam>
    /// <returns></returns>
    public static Task<CommandResult> Map<TSuccess>(
        this ResultBase<TSuccess> result,
        Func<TSuccess, Task<CommandResult>> mapping)
    {
        return result.MapCoreAsync<CommandResult, Unit>(
            mapping,
            f => Task.FromResult(CommandResult.Fail(f)));
    }
     
    /// <summary>
    /// Map internal value to new result type
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TMapped"></typeparam>
    /// <typeparam name="TSuccess"></typeparam>
    /// <returns></returns>
    public static Task<Result<TMapped>> Map<TSuccess, TMapped>(
        this ResultBase<TSuccess> result,
        Func<TSuccess, Task<Result<TMapped>>> mapping)
    {
        return result.MapCoreAsync<Result<TMapped>, TMapped>(
            mapping,
            f => Task.FromResult(Result<TMapped>.Fail(f)));
    }

    /// <summary>
    /// Map internal value to new result type
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TMapped"></typeparam>
    /// <typeparam name="TSuccess"></typeparam>
    /// <returns></returns>
    public static async Task<Result<TMapped>> Map<TSuccess, TMapped>(
        this Task<Result<TSuccess>> result,
        Func<TSuccess, Task<Result<TMapped>>> mapping)
    {
        var priorResult = await result;
        
        return await priorResult.MapCoreAsync<Result<TMapped>, TMapped>(
            mapping,
            f => Task.FromResult(Result<TMapped>.Fail(f)));
    }
     
    /// <summary>
    /// Map internal value to new result type
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TMapped"></typeparam>
    /// <typeparam name="TSuccess"></typeparam>
    /// <returns></returns>
    public static async Task<CommandResult<TMapped>> Map<TSuccess, TMapped>(
        this Task<Result<TSuccess>> result,
        Func<TSuccess, Task<CommandResult<TMapped>>> mapping)
    {
        var priorResult = await result;
            
        return await priorResult.MapCoreAsync<CommandResult<TMapped>, TMapped>(
            mapping,
            f => Task.FromResult(CommandResult<TMapped>.Fail(f)));
    }
    
    /// <summary>
    /// Map internal value to new result type
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TMapped"></typeparam>
    /// <typeparam name="TSuccess"></typeparam>
    /// <returns></returns>
    public static async Task<Result<TMapped>> Map<TSuccess, TMapped>(
        this Task<CommandResult<TSuccess>> result,
        Func<TSuccess, Task<Result<TMapped>>> mapping)
    {
        var priorResult = await result;
             
        return await priorResult.MapCoreAsync<Result<TMapped>, TMapped>(
            mapping,
            f => Task.FromResult(Result<TMapped>.Fail(f)));
    }
      
}