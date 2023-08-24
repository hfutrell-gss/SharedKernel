namespace Shared.Abstractions.Kernel;


/// <summary>
/// A basic result type that reports success or invalidation reasons
/// </summary>
public class Result
{
    /// <summary>
    /// True if the operation was successful
    /// </summary>
    public bool WasSuccessful { get; }

    /// <summary>
    /// A list of reasons that the operation was not successful
    /// </summary>
    public IEnumerable<string>? FailureReasons { get; }


    /// <summary>
    /// The exception if an exception was thrown
    /// </summary>
    public Exception? Exception { get; }

    /// <summary>
    /// A successful result
    /// </summary>
    protected Result()
    {
        WasSuccessful = true;
    }

    /// <summary>
    /// Create a failed result
    /// </summary>
    /// <param name="failureReasons"></param>
    protected Result(IEnumerable<string> failureReasons)
    {
        FailureReasons = failureReasons;
        WasSuccessful = false;
    }

    /// <summary>
    /// Create a failed result when an exception was thrown
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="invalidationReasons"></param>
    protected Result(Exception exception, IEnumerable<string> invalidationReasons) : this(invalidationReasons)
    {
        Exception = exception;
    }

    /// <summary>
    /// Report success
    /// </summary>
    /// <returns></returns>
    public static Result Success()
    {
        return new Result();
    }

    /// <summary>
    /// Fail with the exception thrown and a list of failure reasons
    /// </summary>
    /// <param name="ex"></param>
    /// <param name="failureReason"></param>
    /// <returns></returns>
    public static Result Fail(Exception ex, string failureReason)
    {
        return new Result(ex, new[] { failureReason });
    }

    /// <summary>
    /// Fail with the exception thrown and a list of failure reasons
    /// </summary>
    /// <param name="ex"></param>
    /// <param name="failureReasons"></param>
    /// <returns></returns>
    public static Result Fail(Exception ex, IEnumerable<string> failureReasons)
    {
        return new Result(ex, failureReasons);
    }

    /// <summary>
    /// Fail with a list of failure reasons
    /// </summary>
    /// <param name="failureReason"></param>
    /// <returns></returns>
    public static Result Fail(string failureReason)
    {
        return new Result(new[] { failureReason });
    }
    
    /// <summary>
    /// Fail with a list of failure reasons
    /// </summary>
    /// <param name="failureReasons"></param>
    /// <returns></returns>
    public static Result Fail(IEnumerable<string> failureReasons)
    {
        return new Result(failureReasons);
    }

    /// <summary>
    /// Implicitly convert a result to a <see cref="Task{Result}"/>
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public static implicit operator Task<Result>(Result result) => Task.FromResult(result);
}

/// <summary>
/// A basic result that returns a value
/// </summary>
/// <typeparam name="TResult"></typeparam>
public class Result<TResult> : Result
{
    /// <summary>
    /// The result of the operation
    /// </summary>
    public TResult? ResultValue { get; }

    /// <summary>
    /// Create a successful result
    /// </summary>
    /// <param name="resultValue"></param>
    protected Result(TResult resultValue)
    {
        ResultValue = resultValue;
    }
    
    /// <summary>
    /// Create a failed result
    /// </summary>
    /// <param name="failureReasons"></param>
    protected Result(IEnumerable<string> failureReasons) : base(failureReasons)
    {

    }
        
    /// <summary>
    /// Create a failed result when an exception was thrown
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="invalidationReasons"></param>
    protected Result(Exception exception, IEnumerable<string> invalidationReasons) : base(exception, invalidationReasons)
    {

    }

    /// <summary>
    /// Succeed with the result of the operation
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public static Result<TResult> Success(TResult result)
    {
        return new Result<TResult>(result);
    }

    /// <summary>
    /// Fail with the exception thrown and a list of failure reasons
    /// </summary>
    /// <param name="ex"></param>
    /// <param name="failureReason"></param>
    /// <returns></returns>
    public static Result<TResult> Fail(Exception ex, string failureReason)
    {
        return new Result<TResult>(ex, new[] { failureReason });
    }
    
    /// <summary>
    /// Fail with the exception thrown and a list of failure reasons
    /// </summary>
    /// <param name="ex"></param>
    /// <param name="failureReasons"></param>
    /// <returns></returns>
    public static Result<TResult> Fail(Exception ex, IEnumerable<string> failureReasons)
    {
        return new Result<TResult>(ex, failureReasons.Append(ex.Message));
    }
    
    /// <summary>
    /// Fail with a list of failure reasons
    /// </summary>
    /// <param name="failureReason"></param>
    /// <returns></returns>
    public static Result<TResult> Fail(string failureReason)
    {
        return new Result<TResult>(new[] { failureReason });
    }
        
    /// <summary>
    /// Fail with a list of failure reasons
    /// </summary>
    /// <param name="failureReasons"></param>
    /// <returns></returns>
    public static Result<TResult> Fail(IEnumerable<string> failureReasons)
    {
        return new Result<TResult>(failureReasons);
    }
    /// <summary>
    /// Chains functions on <see cref="Result{TResult}"/>
    /// </summary>
    /// <param name="f"></param>
    /// <typeparam name="TMapped"></typeparam>
    /// <returns><see cref="Result{TResult}"/></returns>
    public Result<TMapped> Bind<TMapped>(Func<TResult, Result<TMapped>> f)
    {
        if (WasSuccessful)
        {
            try
            {
                return f(ResultValue!);
            }
            catch (Exception ex)
            {
                return new Result<TMapped>(new[] {ex.Message, ex.InnerException?.Message ?? "no inner exception"});
            }
        }

        return new Result<TMapped>(FailureReasons!);
    }
    
    /// <summary>
    /// Chains functions on <see cref="Result{TResult}"/>
    /// </summary>
    /// <param name="f"></param>
    /// <typeparam name="TMapped"></typeparam>
    /// <returns><see cref="Result{TResult}"/></returns>
    public Result<TMapped> Map<TMapped>(Func<TResult, TMapped> f)
    {
        if (WasSuccessful)
        {
            try
            {
                return new Result<TMapped>(f(ResultValue!));
            }
            catch (Exception ex)
            {
                return new Result<TMapped>(ex, new [] {ex.Message, ex.InnerException?.Message ?? "no inner exception" });
            }
        }
    
        return new Result<TMapped>(FailureReasons!);
    }

    /// <summary>
    /// Implicitly convert a result to a <see cref="Task{Result}"/>
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public static implicit operator Task<Result<TResult>>(Result<TResult> result) => Task.FromResult(result);
}