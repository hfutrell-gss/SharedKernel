using Newtonsoft.Json;
using Shared.Abstractions.Events;

namespace Shared.Abstractions.EventSourcing.Writing;

/// <summary>
/// Represents an aggregate's state has changed
/// </summary>
/// <param name="AggregateId">The Guid of the aggregate</param>
public abstract record ChangeEvent(Guid AggregateId) : DomainEvent, IChangeEvent
{    
    [JsonProperty]
    public EventSequenceNumber? SequenceNumber { get; private set; }

    public void SetSequence(EventSequenceNumber number)
    {
        SequenceNumber = number;
    }
}