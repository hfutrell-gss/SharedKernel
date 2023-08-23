using Shared.Abstractions.Events;

namespace Shared.Application.EventSourcing.Reading;

public interface IChangeEventTypeMap
{
    Type Map(EventType eventType);
    EventType Map(Type type);
}