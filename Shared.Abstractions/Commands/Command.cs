using MediatR;

namespace Shared.Abstractions.Commands;

/// <summary>
/// Command for CQRS that returns a <see cref="CommandResult"/>
/// </summary>
public abstract record Command : IRequest<CommandResult>;

/// <summary>
/// Command for CQRS that returns a specific result type
/// </summary>
/// <typeparam name="TResult"></typeparam>
public abstract record Command<TResult> : IRequest<CommandResult<TResult>>;