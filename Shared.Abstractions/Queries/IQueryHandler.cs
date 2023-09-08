using MediatR;
// ReSharper disable UnusedMember.Global
#pragma warning disable CS0108, CS0114

namespace Shared.Abstractions.Queries;

/// <summary>
/// Handles a query of the specified query type
/// </summary>
/// <typeparam name="TQuery"></typeparam>
/// <typeparam name="TQueryResult"></typeparam>
public interface IQueryHandler<in TQuery, TQueryResult>
    : IRequestHandler<TQuery, TQueryResult>
    where TQuery : Query<TQueryResult>
{
    /// <summary>
    /// Handle the query
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<TQueryResult> Handle(TQuery request, CancellationToken cancellationToken);
}