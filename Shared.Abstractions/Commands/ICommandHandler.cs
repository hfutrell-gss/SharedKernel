using MediatR;
using Shared.Abstractions.Kernel;

namespace Shared.Abstractions.Commands;

/// <summary>
/// Implement to handle the designated command type
/// </summary>
/// <typeparam name="TCommand"></typeparam>
public interface ICommandHandler<in TCommand> 
    : IRequestHandler<TCommand, Result>
    where TCommand : Command
{
    public Task<Result> Handle(TCommand request, CancellationToken cancellationToken);
}

/// <summary>
/// Implement to handle the designated command type
/// </summary>
/// <typeparam name="TCommand"></typeparam>
/// <typeparam name="TResult"></typeparam>
public interface ICommandHandler<in TCommand, TResult> 
    : IRequestHandler<TCommand, Result<TResult>>
    where TCommand : Command<TResult>, IRequest<Result<TResult>>
{
    public Task<Result<TResult>> Handle(TCommand request, CancellationToken cancellationToken);
}