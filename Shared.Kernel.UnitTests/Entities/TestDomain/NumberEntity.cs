using Shared.Abstractions.Kernel;
using Shared.Kernel.Entities;

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