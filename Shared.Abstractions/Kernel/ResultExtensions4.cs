﻿namespace Shared.Abstractions.Kernel;

/// <summary>
/// Result extension for arity of 4
/// </summary>
public static class ResultExtensions4
{
    /// <summary>
    /// Append resolved execution context
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TAppend"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <returns></returns>
    public static Result<(T1, T2, T3, T4, TAppend)> And<T1, T2, T3, T4, TAppend>(
        this Result<(T1, T2, T3, T4)> result,
        Func<T1, T2, T3, T4, Result<TAppend>> mapping)
    {
        if (result.Failed) return Result<(T1, T2, T3, T4, TAppend)>.Fail(result.FailureDetails);

        try
        {
            var nextResult = mapping(
                result.SuccessValue.Item1,
                result.SuccessValue.Item2,
                result.SuccessValue.Item3,
                result.SuccessValue.Item4
            );

            if (nextResult.Failed) return Result<(T1, T2, T3, T4, TAppend)>.Fail(nextResult.FailureDetails);
            
            return Result<(T1, T2, T3, T4, TAppend)>.Success((
                result.SuccessValue.Item1,
                result.SuccessValue.Item2,
                result.SuccessValue.Item3,
                result.SuccessValue.Item4,
                nextResult.SuccessValue
            ));
        }
        catch (Exception ex)
        {
            return Result<(T1, T2, T3, T4, TAppend)>.Fail(ex);
        }
    }

    /// <summary>
    /// Append resolved execution context
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TAppend"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <returns></returns>
    public static Result<(T1, T2, T3, T4, TAppend)> And<T1, T2, T3, T4, TAppend>(
        this Result<(T1, T2, T3, T4)> result,
        Func<T1, T2, T3, T4, TAppend> mapping)
    {
        if (result.Failed) return Result<(T1, T2, T3, T4, TAppend)>.Fail(result.FailureDetails);

        try
        {
            var nextResult = mapping(
                result.SuccessValue.Item1,
                result.SuccessValue.Item2,
                result.SuccessValue.Item3,
                result.SuccessValue.Item4
            );

            return Result<(T1, T2, T3, T4, TAppend)>.Success((
                result.SuccessValue.Item1,
                result.SuccessValue.Item2,
                result.SuccessValue.Item3,
                result.SuccessValue.Item4,
                nextResult
            ));
        }
        catch (Exception ex)
        {
            return Result<(T1, T2, T3, T4, TAppend)>.Fail(ex);
        }
    }

    /// <summary>
    /// Append resolved execution context
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TAppend"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <returns></returns>
    public static async Task<Result<(T1, T2, T3, T4, TAppend)>> And<T1, T2, T3, T4, TAppend>(
        this Task<Result<(T1, T2, T3, T4)>> result,
        Func<T1, T2, T3, T4, Task<TAppend>> mapping)
    {
        var awaitedResult = await result.ConfigureAwait(false);
        if (awaitedResult.Failed) return Result<(T1, T2, T3, T4, TAppend)>.Fail(awaitedResult.FailureDetails);

        try
        {
            var nextResult = await mapping(
                awaitedResult.SuccessValue.Item1,
                awaitedResult.SuccessValue.Item2,
                awaitedResult.SuccessValue.Item3,
                awaitedResult.SuccessValue.Item4
            ).ConfigureAwait(false);

            return Result<(T1, T2, T3, T4, TAppend)>.Success((
                    awaitedResult.SuccessValue.Item1,
                    awaitedResult.SuccessValue.Item2,
                    awaitedResult.SuccessValue.Item3,
                    awaitedResult.SuccessValue.Item4,
                    nextResult
                )
            );
        }
        catch (Exception ex)
        {
            return Result<(T1, T2, T3, T4, TAppend)>.Fail(ex);
        }
    }

    /// <summary>
    /// Append resolved execution context
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TAppend"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <returns></returns>
    public static async Task<Result<(T1, T2, T3, T4, TAppend)>> And<T1, T2, T3, T4, TAppend>(
        this Task<Result<(T1, T2, T3, T4)>> result,
        Func<T1, T2, T3, T4, Task<Result<TAppend>>> mapping)
    {
        var awaitedResult = await result.ConfigureAwait(false);
        if (awaitedResult.Failed) return Result<(T1, T2, T3, T4, TAppend)>.Fail(awaitedResult.FailureDetails);

        try
        {
            var nextResult = await mapping(
                awaitedResult.SuccessValue.Item1,
                awaitedResult.SuccessValue.Item2,
                awaitedResult.SuccessValue.Item3,
                awaitedResult.SuccessValue.Item4
            ).ConfigureAwait(false);

            if (nextResult.Failed) return Result<(T1, T2, T3, T4, TAppend)>.Fail(nextResult.FailureDetails);

            return Result<(T1, T2, T3, T4, TAppend)>.Success((
                    awaitedResult.SuccessValue.Item1,
                    awaitedResult.SuccessValue.Item2,
                    awaitedResult.SuccessValue.Item3,
                    awaitedResult.SuccessValue.Item4,
                    nextResult.SuccessValue
                )
            );
        }
        catch (Exception ex)
        {
            return Result<(T1, T2, T3, T4, TAppend)>.Fail(ex);
        }
    }

    /// <summary>
    /// Append resolved execution context
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TAppend"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <returns></returns>
    public static async Task<Result<(T1, T2, T3, T4, TAppend)>> And<T1, T2, T3, T4, TAppend>(
        this Result<(T1, T2, T3, T4)> result,
        Func<T1, T2, T3, T4, Task<Result<TAppend>>> mapping)
    {
        if (result.Failed) return Result<(T1, T2, T3, T4, TAppend)>.Fail(result.FailureDetails);

        try
        {
            var nextResult = await mapping(
                result.SuccessValue.Item1,
                result.SuccessValue.Item2,
                result.SuccessValue.Item3,
                result.SuccessValue.Item4
            ).ConfigureAwait(false);

            if (nextResult.Failed) return Result<(T1, T2, T3, T4, TAppend)>.Fail(nextResult.FailureDetails);

            return Result<(T1, T2, T3, T4, TAppend)>.Success((
                result.SuccessValue.Item1,
                result.SuccessValue.Item2,
                result.SuccessValue.Item3,
                result.SuccessValue.Item4,
                nextResult.SuccessValue
            ));
        }
        catch (Exception ex)
        {
            return Result<(T1, T2, T3, T4, TAppend)>.Fail(ex);
        }
    }

    /// <summary>
    /// Append resolved execution context
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="TAppend"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <returns></returns>
    public static async Task<Result<(T1, T2, T3, T4, TAppend)>> And<T1, T2, T3, T4, TAppend>(
        this Result<(T1, T2, T3, T4)> result,
        Func<T1, T2, T3, T4, Task<TAppend>> mapping)
    {
        if (result.Failed) return Result<(T1, T2, T3, T4, TAppend)>.Fail(result.FailureDetails);

        try
        {
            var nextResult = await mapping(
                result.SuccessValue.Item1,
                result.SuccessValue.Item2,
                result.SuccessValue.Item3,
                result.SuccessValue.Item4
            ).ConfigureAwait(false);

            return Result<(T1, T2, T3, T4, TAppend)>.Success((
                result.SuccessValue.Item1,
                result.SuccessValue.Item2,
                result.SuccessValue.Item3,
                result.SuccessValue.Item4,
                nextResult
            ));
        }
        catch (Exception ex)
        {
            return Result<(T1, T2, T3, T4, TAppend)>.Fail(ex);
        }
    }

    /// <summary>
    /// Map prior values into an output
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapping"></param>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <returns></returns>
    public static Result<TOut> Then<T1, T2, T3, T4, T5, TOut>(
        this Result<(T1, T2, T3, T4, T5)> result,
        Func<T1, T2, T3, T4, T5, Result<TOut>> mapping
    )
    {
        if (result.Failed) return Result<TOut>.Fail(result.FailureDetails);

        try
        {
            var nextResult = mapping(
                result.SuccessValue.Item1,
                result.SuccessValue.Item2,
                result.SuccessValue.Item3,
                result.SuccessValue.Item4,
                result.SuccessValue.Item5
            );

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
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <returns></returns>
    public static Result<TOut> Then<T1, T2, T3, T4, T5, TOut>(
        this Result<(T1, T2, T3, T4, T5)> result,
        Func<T1, T2, T3, T4, T5, TOut> mapping
    )
    {
        if (result.Failed) return Result<TOut>.Fail(result.FailureDetails);

        try
        {
            var nextResult = mapping(
                result.SuccessValue.Item1,
                result.SuccessValue.Item2,
                result.SuccessValue.Item3, 
                result.SuccessValue.Item4,
                result.SuccessValue.Item5
            );

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
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <returns></returns>
    public static async Task<Result<TOut>> Then<T1, T2, T3, T4, T5, TOut>(
        this Task<Result<(T1, T2, T3, T4, T5)>> result,
        Func<T1, T2, T3, T4, T5, TOut> mapping
    )
    {
        var awaitedResult = await result.ConfigureAwait(false);

        if (awaitedResult.Failed) return Result<TOut>.Fail(awaitedResult.FailureDetails);

        try
        {
            var nextResult = mapping(
                awaitedResult.SuccessValue.Item1,
                awaitedResult.SuccessValue.Item2,
                awaitedResult.SuccessValue.Item3, 
                awaitedResult.SuccessValue.Item4,
                awaitedResult.SuccessValue.Item5
            );

            return Result<TOut>.Success(nextResult);
        }
        catch (Exception ex)
        {
            return Result<TOut>.Fail(ex);
        }
    }
}