using MediatR;
using Shared.Abstractions.Events;

namespace Shared.Application.Events;

/// <summary>
/// A concrete event bus that sends events
/// </summary>
public class EventBus : IEventBus
{
    private readonly IMediator _mediator;

    public EventBus(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken) 
        where TEvent : DomainEvent
    {
        await _mediator.Publish(@event, cancellationToken).ConfigureAwait(false);
    }
}