using Microsoft.Extensions.DependencyInjection;
using Shared.Abstractions.EventSourcing.Reading;
using Shared.Abstractions.EventSourcing.Writing;
using Shared.Application.Tests.EventSourcing.TestApplication;
using Shared.Application.EventSourcing.Common;
using Shared.Application.Extensions;
using Shared.Infrastructure.InMemory;

namespace Shared.Application.Tests.EventSourcing;

public class EventSourcingTests
{
    [Fact]
    public async Task writes_to_the_event_stream()
    {
        var counter = new Counter();
        counter.IncrementNumber();
        counter.IncrementNumber();

        var writer = AggregateEventStreamWriter;
        await writer.WriteToStream(counter);
        
        Assert.Equal(3, EventStore.Read(counter.Id).Count());
    }

    [Fact]
    public async Task reads_from_the_event_stream()
    {
        var counter = new Counter();
        counter.IncrementNumber();
        counter.IncrementNumber();
    
        var writer = AggregateEventStreamWriter;
        await writer.WriteToStream(counter);

        var events = await AggregateEventStreamReader.ReadEventStream(counter.Id);

        Assert.Equal(3, events.Count());
    }

    [Fact]
    public async Task events_already_stored_are_not_restored()
    {
        var counter = new Counter();
        counter.IncrementNumber();
        counter.IncrementNumber();
        
        var writer = AggregateEventStreamWriter;
        await writer.WriteToStream(counter);
    
        var events = await AggregateEventStreamReader.ReadEventStream(counter.Id);

        var counterAgain = Counter.FromHistory(events);
        counterAgain.IncrementNumber();
        counterAgain.IncrementNumber();

        await writer.WriteToStream(counterAgain);
        Assert.Equal(5, (await AggregateEventStreamReader.ReadEventStream(counter.Id)).Count());
    }

    private readonly IServiceProvider _serviceProvider = new ServiceCollection()
            .AddEventSourcing()
            .AddInMemoryInfrastructure()
            .BuildServiceProvider()
        ;

    private IEventStore EventStore => _serviceProvider.GetService<IEventStore>()!;
    private IAggregateEventStreamWriter AggregateEventStreamWriter => _serviceProvider.GetService<IAggregateEventStreamWriter>()!;
    private IAggregateEventStreamReader AggregateEventStreamReader => _serviceProvider.GetService<IAggregateEventStreamReader>()!;
}

