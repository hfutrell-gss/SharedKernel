using MediatR;

namespace Shared.Abstractions.Queries;

/// <summary>
/// A type for reading information
/// </summary>
/// <typeparam name="TQueryResult"></typeparam>
public record Query<TQueryResult>
    : IRequest<TQueryResult>;