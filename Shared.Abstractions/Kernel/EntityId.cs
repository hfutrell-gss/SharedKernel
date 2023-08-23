namespace Shared.Abstractions.Kernel;

public abstract record EntityId<TId>(TId Id)
{
    public static implicit operator TId(EntityId<TId> id) => id.Id;
}