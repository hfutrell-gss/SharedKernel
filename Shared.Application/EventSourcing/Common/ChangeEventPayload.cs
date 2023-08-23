using Newtonsoft.Json;
using Shared.Abstractions.Events;

namespace Shared.Application.EventSourcing.Common;

public record ChangeEventPayload(Guid AggregateId, EventType EventType, string SerializedPayload)
{
    [JsonProperty] 
    public const int Version = 1;
}