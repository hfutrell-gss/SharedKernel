using Shared.Abstractions.EventSourcing.Reading;
using Shared.Abstractions.EventSourcing.Writing;
using Shared.Abstractions.Kernel;
using Shared.Application.EventSourcing.Common;

namespace Shared.Application.EventSourcing.Reading;

public sealed class AggregateEventStreamReader : IAggregateEventStreamReader
{
    private readonly IEventStore _eventStore;
    private readonly IChangeEventSerializer _serializer;
    private readonly IChangeEventTypeMap _typeMap;

    public AggregateEventStreamReader(IEventStore eventStore, IChangeEventSerializer serializer, IChangeEventTypeMap typeMap)
    {
        _eventStore = eventStore;
        _serializer = serializer;
        _typeMap = typeMap;
    }
    
    public Task<IEnumerable<ChangeEvent>> ReadEventStream(AggregateRootId<Guid> id)
    {
        return Task.FromResult(
            _eventStore
                .Read(id)
                .Select(e => 
                    _serializer.Deserialize(_typeMap.Map(e.EventType), e.SerializedPayload)));
    }
}