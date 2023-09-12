using Newtonsoft.Json;
using Shared.Abstractions.EventSourcing.Writing;

namespace Shared.Application.EventSourcing.Common;

/// <summary>
/// Serialization for streaming change events
/// </summary>
internal class JsonChangeEventSerializer : IChangeEventSerializer
{
    /// <summary>
    /// Serialize events into storable payloads
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public string Serialize(ChangeEvent e)
    {
        return JsonConvert.SerializeObject(e);
    }

    /// <summary>
    /// Deserialize an event from a stored payload
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="serializedEvent"></param>
    /// <returns></returns>
    public ChangeEvent Deserialize(Type eventType, string serializedEvent)
    {
        return (ChangeEvent)JsonConvert.DeserializeObject(serializedEvent, eventType)!;
    }
}