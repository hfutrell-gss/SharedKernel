using MediatR;

namespace Shared.Abstractions.Queries;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TQuery"></typeparam>
/// <typeparam name="TQueryResult"></typeparam>
public interface IQueryHandler<TQuery, TQueryResult>
    : IRequestHandler<TQuery, TQueryResult>
    where TQuery : Query<TQueryResult>
{
    public abstract Task<TQueryResult> Handle(TQuery request, CancellationToken cancellationToken);
}