using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Shared.Abstractions.Commands;
using Shared.Abstractions.Events;
using Shared.Abstractions.EventSourcing.Reading;
using Shared.Abstractions.EventSourcing.Writing;
using Shared.Abstractions.Queries;
using Shared.Application.Commands;
using Shared.Application.Events;
using Shared.Application.EventSourcing.Common;
using Shared.Application.EventSourcing.Reading;
using Shared.Application.EventSourcing.Writing;
using Shared.Application.Queries;

namespace Shared.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainEvents(this IServiceCollection services)
    {
        return services
                .AddTransient<IDomainEventDispatcher, DomainEventDispatcher>()
                .AddTransient<IEventBus, EventBus>()
            ;
    }

    public static IServiceCollection AddCqrs(this IServiceCollection services)
    {
        return services
                .AddDomainEvents()
                .AddTransient<ICommandBus, CommandBus>()
                .AddTransient<IQueryBus, QueryBus>()
            ;
    }

    private static readonly IEnumerable<TypeInfo> ChangeEventTypes = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.DefinedTypes)
            .Where(t => !t.IsAbstract && t.GetInterfaces().Contains(typeof(IChangeEvent)))
        ;
    
    public static IServiceCollection AddEventSourcing(this IServiceCollection services)
    {
        var mapper = new ChangeEventMapper();
        
        foreach (var type in ChangeEventTypes)
        {
            mapper.Add(type);
        }
        
        return services
                .AddCqrs()
                .AddSingleton<IChangeEventTypeMap>(mapper)
                .AddTransient<IAggregateEventStreamWriter, AggregateEventStreamWriter>()
                .AddTransient<IAggregateEventStreamReader, AggregateEventStreamReader>()
                .AddTransient<IChangeEventSerializer, JsonChangeEventSerializer>()
            ;
    }
}