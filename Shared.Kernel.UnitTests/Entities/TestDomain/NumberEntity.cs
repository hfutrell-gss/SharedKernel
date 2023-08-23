using Shared.Abstractions.Events;
using Shared.Kernel.Entities;
using Shared.Kernel.Events;

namespace Shared.Kernel.UnitTests.Entities.TestDomain;

public class NumberEntity : Entity<Guid>
{
    public NumberEntity() : this(Guid.NewGuid())
    {
        
    }
    
    private NumberEntity(Guid id) : base(id)
    {
    }

    public void UpdateNumber(int number)
    {
    }
}

public record NumberUpdatedEvent(int Number) : DomainEvent;