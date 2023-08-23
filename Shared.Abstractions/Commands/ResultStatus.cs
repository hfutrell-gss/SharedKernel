namespace Shared.Abstractions.Commands;

/// <summary>
/// Status of the result
/// </summary>
public enum ResultStatus
{
    /// <summary>
    /// Status not set
    /// </summary>
    Unknown,
    
    /// <summary>
    /// Result succeeded
    /// </summary>
    Success,
    
    /// <summary>
    /// Result failed
    /// </summary>
    Failed,
}