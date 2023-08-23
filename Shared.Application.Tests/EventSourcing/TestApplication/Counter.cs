using Shared.Abstractions.EventSourcing.Writing;
using Shared.Kernel.EventSourcing;

namespace Shared.Application.Tests.EventSourcing.TestApplication;

public class Counter : EventSourcedAggregateRoot<Counter, CounterId>
{
    private int Number { get; set; }

    private Counter(CounterId id) : base(id)
    {
        RegisterChangeHandler<CounterCreatedEvent>(CounterCreatedEventHandler);
        RegisterChangeHandler<NumberIncrementedEvent>(NumberUpdatedChangeHandler);
    }

    private ChangeResult<Counter> CounterCreatedEventHandler(CounterCreatedEvent arg)
    {
        return Success();
    }

    public Counter() : this(new CounterId(Guid.NewGuid()))
    {
        TryDoChange(new CounterCreatedEvent(Id));
    }

    public ChangeResult<Counter> IncrementNumber()
    {
        return TryDoChange(new NumberIncrementedEvent(Id));
    }
    
    private ChangeResult<Counter> NumberUpdatedChangeHandler(NumberIncrementedEvent arg)
    {
        Number++;
 
        return Success();
    }

    public static Counter FromHistory(IEnumerable<ChangeEvent> events)
    {
        return Rehydrate(id => new Counter(id), events);
    }
   
}