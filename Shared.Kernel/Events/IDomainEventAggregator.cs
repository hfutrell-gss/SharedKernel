using Shared.Abstractions.Events;
using Shared.Abstractions.Kernel;

namespace Shared.Kernel.Events;

internal interface IDomainEventAggregator
{
    IReadOnlyCollection<DomainEvent> Events { get; }
    void Add(DomainEvent @event);
    void ClearDomainEvents();
}