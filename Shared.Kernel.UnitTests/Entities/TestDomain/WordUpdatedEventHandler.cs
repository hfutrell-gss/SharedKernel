using Shared.Abstractions.Events;

namespace Shared.Kernel.UnitTests.Entities.TestDomain;

public class WordUpdatedEventHandler : IDomainEventHandler<WordUpdatedEvent>
{
    private readonly WordListener _wordListener;

    public WordUpdatedEventHandler(WordListener wordListener)
    {
        _wordListener = wordListener;
    }
    
    public Task Handle(WordUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _wordListener.Word = notification.Word;
            
        return Task.CompletedTask;
    }
}

public class NumberUpdatedEventHandler : IDomainEventHandler<NumberUpdatedEvent>
{
    private readonly NumberListener _numberListener;

    public NumberUpdatedEventHandler(NumberListener numberListener)
    {
        _numberListener = numberListener;
    }
    
    public Task Handle(NumberUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _numberListener.Number = notification.Number;
        _numberListener.Numbers.Push(notification.Number);
            
        return Task.CompletedTask;
    }
}

public class NumberListener
{
    public int Number { get; set; }
    public Stack<int> Numbers { get; } = new();
}