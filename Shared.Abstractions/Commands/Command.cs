using MediatR;
using Shared.Abstractions.Kernel;

namespace Shared.Abstractions.Commands;

/// <summary>
/// Command for CQRS that returns a <see cref="Result"/>
/// </summary>
public abstract record Command : IRequest<Result>;

/// <summary>
/// Command for CQRS that returns a specific result type
/// </summary>
/// <typeparam name="TResult"></typeparam>
public abstract record Command<TResult> : IRequest<Result<TResult>>;