using Shared.Abstractions.EventSourcing.Writing;
using Shared.Abstractions.Kernel;
using Shared.Application.EventSourcing.Common;
using Shared.Application.EventSourcing.Reading;

namespace Shared.Application.EventSourcing.Writing;

public sealed class AggregateEventStreamWriter : IAggregateEventStreamWriter
{
    private readonly IEventStore _eventStore;
    private readonly IChangeEventSerializer _serializer;
    private readonly IChangeEventTypeMap _typeMap;

    public AggregateEventStreamWriter(IEventStore eventStore, IChangeEventSerializer serializer, IChangeEventTypeMap typeMap)
    {
        _eventStore = eventStore;
        _serializer = serializer;
        _typeMap = typeMap;
    }

    public async Task<bool> WriteToStream<TId>(IEventSourcedAggregateRoot<TId> aggregate)
        where TId : AggregateRootId<Guid>
    {
        return await WriteToStream(aggregate.Id, aggregate.EventSourcingEvents);
    }
    
    public async Task<bool> WriteToStream(AggregateRootId<Guid> aggregateId, IEnumerable<ChangeEvent> events)
    {
        foreach (var changeEvent in events)
        {
            var writeableChangeEvent = new ChangeEventPayload(
                aggregateId,
                _typeMap.Map(changeEvent.GetType()),
                _serializer.Serialize(changeEvent)
            );

            await _eventStore.Write(writeableChangeEvent);
        }

        return true;
    }
}