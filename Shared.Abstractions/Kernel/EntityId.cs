namespace Shared.Abstractions.Kernel;

/// <summary>
/// Base object for ids used by entity types
/// </summary>
/// <param name="Id"></param>
/// <typeparam name="TId"></typeparam>
public abstract record EntityId<TId>(TId Id)
{
    /// <summary>
    /// Implicitly convert an Id to its wrapped type
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static implicit operator TId(EntityId<TId> id) => id.Id;
}

