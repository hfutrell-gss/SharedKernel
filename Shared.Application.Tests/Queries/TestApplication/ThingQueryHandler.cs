using Shared.Abstractions.Queries;

namespace Shared.Application.Tests.Queries.TestApplication;

public class ThingQueryHandler : IQueryHandler<ThingQuery, ThingQueryResult>
{
    private readonly ThingStore _store;

    public ThingQueryHandler(ThingStore store)
    {
        _store = store;
    }
    
    public Task<ThingQueryResult> Handle(ThingQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new ThingQueryResult(_store.GetThing(request.ThingToGet)));
    }
}