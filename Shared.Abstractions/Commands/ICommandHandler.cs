using MediatR;
using Shared.Abstractions.Results;
#pragma warning disable CS0108, CS0114

namespace Shared.Abstractions.Commands;

/// <summary>
/// Implement to handle the designated command type
/// </summary>
/// <typeparam name="TCommand"></typeparam>
public interface ICommandHandler<in TCommand> 
    : IRequestHandler<TCommand, Result>
    where TCommand : Command
{
    /// <summary>
    /// Handle the command's execution
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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
    /// <summary>
    /// Handle the command's execution
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<Result<TResult>> Handle(TCommand request, CancellationToken cancellationToken);
}