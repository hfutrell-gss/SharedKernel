namespace Shared.Abstractions.Kernel;

public record AggregateRootId<TId>(TId Id) : EntityId<TId>(Id);