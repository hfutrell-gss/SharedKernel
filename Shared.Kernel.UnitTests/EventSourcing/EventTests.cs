using Shared.Abstractions.Events;
using Shared.Kernel.UnitTests.EventSourcing.TestDomain;

namespace Shared.Kernel.UnitTests.EventSourcing;

public class EventTests
{
    [Fact]
    public void have_a_version_number()
    {
        var e = new OrchardCreatedEvent(new OrchardId(Guid.NewGuid()), "x");
        
        Assert.True(DomainEvent.DomainEventVersion > 0);
    }
    
    [Fact]
    public void have_a_unique_id()
    {
        var e = new OrchardCreatedEvent(new OrchardId(Guid.NewGuid()), "x");
            
        Assert.NotEqual(Guid.Empty, e.EventId);
    }
    
    [Fact]
    public void have_an_occurred_time()
    {
        var e = new OrchardCreatedEvent(new OrchardId(Guid.NewGuid()), "x");
                
        Assert.InRange(DateTime.UtcNow - e.WhenEventOccurred, TimeSpan.Zero, TimeSpan.FromMilliseconds(10));
    }
}