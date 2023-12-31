using MediatR;
using Shared.Abstractions.Queries;

namespace Shared.Application.Queries;

internal sealed class QueryBus : IQueryBus
{
    private readonly IMediator _mediator;

    public QueryBus(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<TResult> SendQuery<TQuery, TResult>(TQuery query) where TQuery 
        : Query<TResult>
    {
        return await _mediator.Send(query).ConfigureAwait(false);
    }
}