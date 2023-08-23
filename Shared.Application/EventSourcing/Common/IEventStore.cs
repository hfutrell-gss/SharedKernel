using Shared.Abstractions.Kernel;

namespace Shared.Application.EventSourcing.Common;

/// <summary>
/// For storing an event stream
/// </summary>
public interface IEventStore
{
    public Task<bool> Write(ChangeEventPayload @event);
    IEnumerable<ChangeEventPayload> Read(AggregateRootId<Guid> id);
}