using Shared.Abstractions.Kernel;

namespace Shared.Abstractions.EventSourcing.Writing;

/// <summary>
/// Represents an new aggregate entity has been created
/// </summary>
/// <param name="Id">Aggregate Id</param>
/// <typeparam name="T"></typeparam>
public abstract record CreationEvent<T>(T Id) : ChangeEvent(Id) where T : AggregateRootId<Guid>
{
    /// <summary>
    /// Unique Id of the creation event. Corresponds to the
    /// Id of the aggregate because only one can exist for
    /// the aggregate
    /// </summary>
    public override Guid EventId { get; } = Id;
};