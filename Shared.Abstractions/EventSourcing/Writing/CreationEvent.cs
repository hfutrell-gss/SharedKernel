using System.Diagnostics.CodeAnalysis;
using Shared.Abstractions.Kernel;

namespace Shared.Abstractions.EventSourcing.Writing;

/// <summary>
/// Represents an new aggregate entity has been created
/// </summary>
/// <param name="Id">Aggregate Id</param>
/// <typeparam name="T"></typeparam>
[ExcludeFromCodeCoverage]
public abstract record CreationEvent<T>(T Id) : ChangeEvent(Id) where T : AggregateRootId<Guid>;