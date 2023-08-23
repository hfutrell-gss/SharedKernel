using Microsoft.Extensions.DependencyInjection;
using Shared.Abstractions.Queries;
using Shared.Application.Tests.Queries.TestApplication;
using Shared.Application.Extensions;

namespace Shared.Application.Tests.Queries;

public class QueryTests
{
    private readonly ServiceProvider _serviceProvider;
    private const int GoodResult = 1;

    public QueryTests()
    {
        _serviceProvider = new ServiceCollection()
                .AddMediatR(c => c.RegisterServicesFromAssembly(typeof(ThingQueryHandler).Assembly))
                .AddCqrs()
                .AddSingleton<ThingStore>()
                .BuildServiceProvider()
            ;
    }

    private async Task<int> RunGoodQuery()
    {
        var result = await _serviceProvider.GetService<IQueryBus>()!.SendQuery<ThingQuery, ThingQueryResult>(new ThingQuery(0));
        return result.ThingGotten;
    }

    [Fact]
    public async Task gets_the_thing()
    {
        var result = await RunGoodQuery();
        Assert.Equal(GoodResult, result);
    }
}