using Shared.Abstractions.Kernel;
using Shared.Kernel.Events;

namespace Shared.Kernel.Entities;

/// <summary>
/// An aggregate root per DDD
/// </summary>
/// <typeparam name="TId"></typeparam>
/// <typeparam name="TIdType"></typeparam>
public abstract class AggregateRoot<TId, TIdType> 
    : Entity<TId>,
        IAggregateRoot where TId : AggregateRootId<TIdType>
{
   
    /// <summary>
    /// An Id that enforces uniqueness
    /// <br/>
    /// <br/>
    /// A <see cref="Guid"/> type is recommended but left
    /// available to the user
    /// </summary>
    public new TId Id { get; } = null!;

    /// <inheritdoc/>>
    public IReadOnlyCollection<DomainEvent> Events => _domainEventAggregator.Events;

    /// <inheritdoc/>>
    public void ClearEvents()
    {
        _domainEventAggregator.ClearDomainEvents();    
    }

    private readonly IDomainEventAggregator _domainEventAggregator;
 
    /// <summary>
    /// Requires an Id for consistency with the underlying event systems
    /// </summary>
    /// <param name="id"></param>
    protected AggregateRoot(TId id) : this(id, new DomainEventAggregator())
    {
        Id = id;
    }

    private AggregateRoot(TId id, IDomainEventAggregator eventAggregator) : base(id)
    {
        _domainEventAggregator = eventAggregator;
    }
    
    /// <summary>
    /// Registers an event for later disbursement
    /// </summary>
    /// <param name="event"></param>
    protected void RegisterDomainEvent(DomainEvent @event)
    {
        _domainEventAggregator.Add(@event);     
    }

}