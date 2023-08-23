using Shared.Abstractions.Events;

namespace Shared.Kernel.Events;

internal interface IDomainEventAggregator
{
    IReadOnlyCollection<DomainEvent> Events { get; }
    void Add(DomainEvent @event);
    void ClearDomainEvents();
}