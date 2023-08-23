using Shared.Abstractions.EventSourcing.Writing;

namespace Shared.Application.EventSourcing.Common;

public interface IChangeEventSerializer
{
    public string Serialize(ChangeEvent e);
    public ChangeEvent Deserialize(Type eventType, string serializedEvent);
}