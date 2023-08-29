using MediatR;
using Shared.Abstractions.Kernel;

namespace Shared.Abstractions.Events;

/// <summary>
/// For receiving and acting on domain events
/// </summary>
/// <typeparam name="TDomainEvent"></typeparam>
public interface IDomainEventHandler<in TDomainEvent> 
    : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
}
