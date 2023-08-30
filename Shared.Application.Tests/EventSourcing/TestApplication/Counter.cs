using Shared.Abstractions.EventSourcing.Writing;
using Shared.Abstractions.Kernel;
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

    private Result<Counter> CounterCreatedEventHandler(CounterCreatedEvent arg)
    {
        return Success();
    }

    public Counter() : this(new CounterId(Guid.NewGuid()))
    {
        TryDoChange(new CounterCreatedEvent(Id));
    }

    public Result<Counter> IncrementNumber()
    {
        return TryDoChange(new NumberIncrementedEvent(Id));
    }
    
    private Result<Counter> NumberUpdatedChangeHandler(NumberIncrementedEvent arg)
    {
        Number++;
 
        return Success();
    }

    public static Counter FromHistory(IEnumerable<ChangeEvent> events)
    {
        return Rehydrate(id => new Counter(id), events);
    }
   
}