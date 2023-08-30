using MediatR;
using Shared.Abstractions.Commands;
using Shared.Abstractions.Kernel;

namespace Shared.Application.Commands;

/// <summary>
/// Concrete command bus for delivering commands
/// </summary>
public class CommandBus : ICommandBus
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Uses a mediator as the bus
    /// </summary>
    /// <param name="mediator"></param>
    public CommandBus(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Send a command to the bus
    /// </summary>
    /// <param name="command"></param>
    /// <typeparam name="TCommand"></typeparam>
    /// <returns></returns>
    public async Task<Result> SendCommand<TCommand>(TCommand command) 
        where TCommand : Command
    {
        return await _mediator.Send(command).ConfigureAwait(false);
    }

    /// <summary>
    /// Send a command with a nonstandard result to the bus
    /// </summary>
    /// <param name="command"></param>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public async Task<Result<TResult>> SendCommand<TCommand, TResult>(TCommand command) 
        where TCommand : Command<TResult> 
    {
        return await _mediator.Send(command).ConfigureAwait(false);
    }
}