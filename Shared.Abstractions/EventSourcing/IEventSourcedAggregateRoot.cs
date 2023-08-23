using Shared.Abstractions.EventSourcing.Writing;
using Shared.Abstractions.Kernel;

namespace Shared.Abstractions.EventSourcing;

/// <summary>
/// An aggregate root that stores event sourcing specific events
/// </summary>
public interface IEventSourcedAggregateRoot : IAggregateRoot
{
    /// <summary>
    /// Events used to rebuild an aggregate's state
    /// </summary>
    public IReadOnlyCollection<ChangeEvent> EventSourcingEvents { get; }
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="TId"></typeparam>
public interface IEventSourcedAggregateRoot<out TId> : IEventSourcedAggregateRoot
    where TId : AggregateRootId<Guid>
{
    /// <summary>
    /// The event sourced aggregate roots Id
    /// </summary>
    public TId Id { get; }
}