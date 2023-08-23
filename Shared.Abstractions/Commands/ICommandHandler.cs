using MediatR;

namespace Shared.Abstractions.Commands;

public interface ICommandHandler<in TCommand> 
    : IRequestHandler<TCommand, CommandResult>
    where TCommand : Command
{
    public Task<CommandResult> Handle(TCommand request, CancellationToken cancellationToken);
}

public interface ICommandHandler<in TCommand, TResult> 
    : IRequestHandler<TCommand, TResult>
    where TCommand : Command<TResult>
    where TResult : CommandResult
{
    public Task<TResult> Handle(TCommand request, CancellationToken cancellationToken);
}