using Shared.Abstractions.Kernel;

namespace Shared.Kernel.UnitTests.EventSourcing.TestDomain;

public record OrchardId(Guid Id) : AggregateRootId<Guid>(Id);