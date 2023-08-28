using MediatR;

namespace Shared.Abstractions.Commands;

/// <summary>
/// Implement to handle the designated command type
/// </summary>
/// <typeparam name="TCommand"></typeparam>
public interface ICommandHandler<in TCommand> 
    : IRequestHandler<TCommand, CommandResult>
    where TCommand : Command
{
    public Task<CommandResult> Handle(TCommand request, CancellationToken cancellationToken);
}

/// <summary>
/// Implement to handle the designated command type
/// </summary>
/// <typeparam name="TCommand"></typeparam>
/// <typeparam name="TResult"></typeparam>
public interface ICommandHandler<in TCommand, TResult> 
    : IRequestHandler<TCommand, CommandResult<TResult>>
    where TCommand : Command<TResult>, IRequest<CommandResult<TResult>>
{
    public Task<CommandResult<TResult>> Handle(TCommand request, CancellationToken cancellationToken);
}