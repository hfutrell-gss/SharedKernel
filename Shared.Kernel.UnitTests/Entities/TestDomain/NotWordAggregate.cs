using Shared.Kernel.Entities;

namespace Shared.Kernel.UnitTests.Entities.TestDomain;

internal class NotWordAggregate : AggregateRoot<NotWordAggregateId, Guid>
{
    public NotWordAggregate(NotWordAggregateId id) : base(id)
    {
    }
}