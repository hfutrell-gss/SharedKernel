using Shared.Abstractions.Kernel;

namespace Shared.Abstractions.EventSourcing.Writing;

/// <summary>
/// Access a stream to write events
/// </summary>
public interface IAggregateEventStreamWriter
{
    /// <summary>
    /// Append the events to a stream
    /// </summary>
    /// <returns></returns>
    Task<bool> WriteToStream<TId>(IEventSourcedAggregateRoot<TId> aggregate) 
        where TId : AggregateRootId<Guid>;
}
