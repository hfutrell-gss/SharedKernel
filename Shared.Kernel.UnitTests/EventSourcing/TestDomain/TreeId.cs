using Shared.Abstractions.Kernel;

namespace Shared.Kernel.UnitTests.EventSourcing.TestDomain;

public record TreeId(Guid Id) : AggregateRootId<Guid>(Id);