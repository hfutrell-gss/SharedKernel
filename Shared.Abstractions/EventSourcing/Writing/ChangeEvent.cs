using Newtonsoft.Json;
using Shared.Abstractions.Kernel;

namespace Shared.Abstractions.EventSourcing.Writing;

/// <summary>
/// Represents an aggregate's state has changed
/// </summary>
/// <param name="AggregateId">The Guid of the aggregate</param>
public abstract record ChangeEvent(Guid AggregateId) : DomainEvent, IChangeEvent
{    
    /// <summary>
    /// The order of the event in an aggregate's event sequence
    /// </summary>
    [JsonProperty]
    public EventSequenceNumber? SequenceNumber { get; private set; }

    /// <summary>
    /// Apply the sequence number
    /// </summary>
    /// <param name="number"></param>
    public void SetSequence(EventSequenceNumber number)
    {
        SequenceNumber = number;
    }
}