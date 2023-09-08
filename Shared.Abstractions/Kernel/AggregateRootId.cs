namespace Shared.Abstractions.Kernel;

/// <summary>
/// The Id for an Aggregate Root
/// </summary>
/// <param name="Id"></param>
/// <typeparam name="TId"></typeparam>
public record AggregateRootId<TId>(TId Id) : EntityId<TId>(Id);