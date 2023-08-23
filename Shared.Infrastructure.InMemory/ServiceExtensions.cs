using Microsoft.Extensions.DependencyInjection;
using Shared.Application.EventSourcing.Common;
using Shared.Infrastructure.InMemory.Events;

namespace Shared.Infrastructure.InMemory;

public static class ServiceExtensions
{
    public static IServiceCollection AddInMemoryInfrastructure(this IServiceCollection services)
    {
        return services
                .AddSingleton<IEventStore, InMemoryEventStore>()
            ;
    }
}