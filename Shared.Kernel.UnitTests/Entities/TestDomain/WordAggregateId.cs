using Shared.Abstractions.Kernel;

namespace Shared.Kernel.UnitTests.Entities.TestDomain;

internal record WordAggregateId(Guid Id) : AggregateRootId<Guid>(Id)
{
    public static WordAggregateId New()
    {
        return new WordAggregateId(Guid.NewGuid());
    }
};