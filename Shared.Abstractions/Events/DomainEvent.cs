using MediatR;

namespace Shared.Abstractions.Events;

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
    public virtual Guid EventId { get; } = Guid.NewGuid();
    
    /// <summary>
    /// When the event occured
    /// </summary>
    public DateTime WhenEventOccurred { get; } = DateTime.UtcNow;
}