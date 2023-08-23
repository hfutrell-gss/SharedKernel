using Shared.Abstractions.Kernel;

namespace Shared.Kernel.UnitTests.Entities.TestDomain;

internal record NotWordAggregateId(Guid Id) : AggregateRootId<Guid>(Id);