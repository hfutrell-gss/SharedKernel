using Shared.Abstractions.Events;
using Shared.Application.EventSourcing.Reading;

namespace Shared.Application.Extensions;

internal class ChangeEventMapper : IChangeEventTypeMap
{
    private readonly Dictionary<EventType, Type> _map = new();
    private readonly Dictionary<Type, EventType> _coMap = new();

    public void Add(Type t)
    {
        _map.Add(t, t);
        _coMap.Add(t, t);
    }
    
    public Type Map(EventType eventType)
    {
        return _map[eventType];
    }

    public EventType Map(Type type)
    {
        return _coMap[type];
    }
}