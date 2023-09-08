namespace Shared.Abstractions.Results;

public record FailureDetails
{
    private readonly string[] _failureReasons;

    public IReadOnlyCollection<string> FailureReasons => _failureReasons;
    
    public Exception? Exception { get; }
     
    private FailureDetails(string[] failureReasons)
    {
        _failureReasons = failureReasons;
    }

    private FailureDetails(Exception exception, string[] failureReasons) : this(failureReasons)
    {
        Exception = exception;
    }

    public static FailureDetails From(params string[] reasons)
    {
        return new FailureDetails(reasons);
    }

    public static FailureDetails From(Exception exception)
    {
        var reasons = new [] { exception.Message, exception.InnerException?.Message ?? "No inner exception" };
        
        return new FailureDetails(exception, reasons);
    }
    
    public static FailureDetails From(Exception exception, params string[] reasons)
    {
        return new FailureDetails(exception, reasons);
    }
}