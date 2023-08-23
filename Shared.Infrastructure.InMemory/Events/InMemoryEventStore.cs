using Newtonsoft.Json;
using Shared.Abstractions.Kernel;
using Shared.Application.EventSourcing.Common;

namespace Shared.Infrastructure.InMemory.Events;

public class InMemoryEventStore : IEventStore
{
    private readonly Dictionary<string, Dictionary<Guid, List<string>>> _eventStreams = new();

    public IEnumerable<ChangeEventPayload> GetEvents(Guid aggregateId) =>
        _eventStreams.Values.SelectMany(stream => 
            stream.ContainsKey(aggregateId) ?
                stream[aggregateId].Select(JsonConvert.DeserializeObject<ChangeEventPayload>).ToList()! :
                new List<ChangeEventPayload>());


    public Task<bool> Write(ChangeEventPayload @event)
    {
        if (!_eventStreams.ContainsKey(@event.EventType))
        {
            _eventStreams.Add(@event.EventType, new Dictionary<Guid, List<string>>());
        }

        if (!_eventStreams[@event.EventType].ContainsKey(@event.AggregateId))
        {
            _eventStreams[@event.EventType].Add(@event.AggregateId, new List<string>());
        }

        var eventStream = _eventStreams[@event.EventType][@event.AggregateId];
        
        eventStream.Add(JsonConvert.SerializeObject(@event));

        return Task.FromResult(true);
    }

    public IEnumerable<ChangeEventPayload> Read(AggregateRootId<Guid> id)
    {
        return GetEvents(id);
    }
}