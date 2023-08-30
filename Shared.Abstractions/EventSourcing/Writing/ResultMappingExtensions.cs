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
    public static async Task<Result<TMapped>> Map<TSuccess, TMapped>(
        this Task<Result<TSuccess>> result,
        Func<TSuccess, Task<Result<TMapped>>> mapping)
    {
        var priorResult = await result;
             
        return await priorResult.MapCoreAsync<Result<TMapped>, TMapped>(
            mapping,
            f => Task.FromResult(Result<TMapped>.Fail(f)));
    }
      
}