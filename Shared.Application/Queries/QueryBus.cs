using MediatR;
using Shared.Abstractions.Queries;

namespace Shared.Application.Queries;

public class QueryBus : IQueryBus
{
    private readonly IMediator _mediator;

    public QueryBus(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<TResult> SendQuery<TQuery, TResult>(TQuery query) where TQuery 
        : Query<TResult> where TResult : QueryResult
    {
        return await _mediator.Send(query).ConfigureAwait(false);
    }
}