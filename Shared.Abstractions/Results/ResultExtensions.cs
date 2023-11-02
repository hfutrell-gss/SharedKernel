namespace Shared.Abstractions.Results;

/// <summary>
/// Extensions for Result Calculations
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Append resolved execution context
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TAppend"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <returns></returns>
    public static Result<(T1, TAppend)> And<T1, TAppend>(
        this Result<T1> result,
        Func<T1, Result<TAppend>> mapping)
    {
        if (result.Failed) return Result<(T1, TAppend)>.Fail(result.FailureDetails);
        
        var nextResult = result.MapCore<Result<TAppend>, TAppend>(
            mapping,
            Result<TAppend>.Fail);
    
        if (nextResult.Failed) return Result<(T1, TAppend)>.Fail(nextResult.FailureDetails);
             
        return Result<(T1, TAppend)>.Success((result.SuccessValue, nextResult.SuccessValue));
    }

    /// <summary>
    /// Append resolved execution context
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TAppend"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <returns></returns>
    public static Result<(T1, TAppend)> And<T1, TAppend>(
        this Result<T1> result,
        Func<T1, TAppend> mapping)
    {
        if (result.Failed) return Result<(T1, TAppend)>.Fail(result.FailureDetails);

        try
        {
            var nextResult = mapping(result.SuccessValue);
            
            return Result<(T1, TAppend)>.Success((result.SuccessValue, nextResult));
        }
        catch (Exception ex)
        {
            return Result<(T1, TAppend)>.Fail(ex);
        }
    }

    /// <summary>
    /// Append resolved execution context
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TAppend"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <returns></returns>
    public static async Task<Result<(T1, TAppend)>> And<T1, TAppend>(
        this Task<Result<T1>> result,
        Func<T1, Task<TAppend>> mapping)
    {
        var awaitedResult = await result.ConfigureAwait(false);
        if (awaitedResult.Failed) return Result<(T1, TAppend)>.Fail(awaitedResult.FailureDetails);
        
        try
        {
            var nextResult = await mapping(awaitedResult.SuccessValue).ConfigureAwait(false);
            
            return Result<(T1, TAppend)>.Success((
                    awaitedResult.SuccessValue,
                    nextResult)
            );
        }
        catch (Exception ex)
        {
            return Result<(T1, TAppend)>.Fail(ex);
        }
    }
        
    /// <summary>
    /// Append resolved execution context
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TAppend"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <returns></returns>
    public static async Task<Result<(T1, TAppend)>> And<T1, TAppend>(
        this Task<Result<T1>> result,
        Func<T1, Task<Result<TAppend>>> mapping)
    {
        var awaitedResult = await result.ConfigureAwait(false);
        if (awaitedResult.Failed) return Result<(T1, TAppend)>.Fail(awaitedResult.FailureDetails);
        
        try
        {
            var nextResult = await mapping(awaitedResult.SuccessValue).ConfigureAwait(false);
        
            if (nextResult.Failed) return Result<(T1, TAppend)>.Fail(nextResult.FailureDetails);
            
            return Result<(T1, TAppend)>.Success((
                    awaitedResult.SuccessValue,
                    nextResult.SuccessValue)
            );
        }
        catch (Exception ex)
        {
            return Result<(T1, TAppend)>.Fail(ex);
        }
    }
         
    /// <summary>
    /// Append resolved execution context
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TAppend"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <returns></returns>
    public static async Task<Result<(T1, TAppend)>> And<T1, TAppend>(
        this Result<T1> result,
        Func<T1, Task<Result<TAppend>>> mapping)
    {
        if (result.Failed) return Result<(T1, TAppend)>.Fail(result.FailureDetails);
            
        try
        {
            var nextResult = await mapping(result.SuccessValue).ConfigureAwait(false);
            
            if (nextResult.Failed) return Result<(T1, TAppend)>.Fail(nextResult.FailureDetails);
            
            return Result<(T1, TAppend)>.Success((result.SuccessValue, nextResult.SuccessValue));
        }
        catch (Exception ex)
        {
            return Result<(T1, TAppend)>.Fail(ex);
        }
    }
         
    /// <summary>
    /// Append resolved execution context
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TAppend"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <returns></returns>
    public static async Task<Result<(T1, TAppend)>> And<T1, TAppend>(
        this Result<T1> result,
        Func<T1, Task<TAppend>> mapping)
    {
        if (result.Failed) return Result<(T1, TAppend)>.Fail(result.FailureDetails);
                    
        try
        {
            var nextResult = await mapping(result.SuccessValue).ConfigureAwait(false);
            
            return Result<(T1, TAppend)>.Success((result.SuccessValue, nextResult));
        }
        catch (Exception ex)
        {
            return Result<(T1, TAppend)>.Fail(ex);
        }
    }
    
    /// <summary>
    /// Append resolved execution context
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TAppend"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <returns></returns>
    public static async Task<Result<(T1, TAppend)>> And<T1, TAppend>(
        this Task<Result<T1>> result,
        Func<T1, TAppend> mapping)
    {
        var awaitedResult = await result.ConfigureAwait(false);
        if (awaitedResult.Failed) return Result<(T1, TAppend)>.Fail(awaitedResult.FailureDetails);
                    
        try
        {
            var nextResult = mapping(awaitedResult.SuccessValue);
            
            return Result<(T1, TAppend)>.Success((awaitedResult.SuccessValue, nextResult));
        }
        catch (Exception ex)
        {
            return Result<(T1, TAppend)>.Fail(ex);
        }
    }

    /// <summary>
    /// Map prior values into an output
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <returns></returns>
    public static Result<TOut> Then<T1, T2, TOut>(
        this Result<(T1, T2)> result,
        Func<T1, T2, Result<TOut>> mapping
    )
    {
        if (result.Failed) return Result<TOut>.Fail(result.FailureDetails);
        
        try
        {
            var nextResult = mapping(result.SuccessValue.Item1, result.SuccessValue.Item2);
        
            return nextResult;
        }
        catch (Exception ex)
        {
            return Result<TOut>.Fail(ex);
        }
        
    }       
            
    /// <summary>
    /// Map prior values into an output
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <returns></returns>
    public static Result<TOut> Then<T1, T2, TOut>(
        this Result<(T1, T2)> result,
        Func<T1, T2, TOut> mapping
    )
    {
        if (result.Failed) return Result<TOut>.Fail(result.FailureDetails);
                
        try
        {
            var nextResult = mapping(result.SuccessValue.Item1, result.SuccessValue.Item2);
                
            return Result<TOut>.Success(nextResult);
        }
        catch (Exception ex)
        {
            return Result<TOut>.Fail(ex);
        }
    }
     
    /// <summary>
    /// Map prior values into an output
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <returns></returns>
    public static async Task<Result<TOut>> Then<T1, T2, TOut>(
        this Task<Result<(T1, T2)>> result,
        Func<T1, T2, TOut> mapping
    )
    {
        var awaitedResult = await result.ConfigureAwait(false);
        
        if (awaitedResult.Failed) return Result<TOut>.Fail(awaitedResult.FailureDetails);
                    
        try
        {
            var nextResult = mapping(awaitedResult.SuccessValue.Item1, awaitedResult.SuccessValue.Item2);
                    
            return Result<TOut>.Success(nextResult);
        }
        catch (Exception ex)
        {
            return Result<TOut>.Fail(ex);
        }
    }
     
}