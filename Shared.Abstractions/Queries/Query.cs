using MediatR;

namespace Shared.Abstractions.Queries;

public record Query<TQueryResult>
    : IRequest<TQueryResult>
    where TQueryResult : QueryResult;