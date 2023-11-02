using MediatR;

namespace Shared.Abstractions.Kernel;

/// <summary>
/// Marker interface to represent a domain event
/// </summary>
public interface IDomainEvent : INotification {}

/// <summary>
/// Captures an event from the domain that can
/// be propagated outward
/// </summary>
public abstract record DomainEvent : IDomainEvent
{
    /// <summary>
    /// For general event versioning
    /// </summary>
    public const int DomainEventVersion = 1;
        
    /// <summary>
    /// The unique id of the event
    /// </summary>
    public Guid EventId { get; private set; } = Guid.NewGuid();
    
    /// <summary>
    /// When the event occured
    /// </summary>
    public DateTime WhenEventOccurred { get; private set; } = DateTime.UtcNow;
}