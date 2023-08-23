namespace Shared.Abstractions.Commands;

/// <summary>
/// Delivers commands to the bus
/// </summary>
public interface ICommandBus
{
    /// <summary>
    /// Dispatch a command to the bus
    /// </summary>
    /// <param name="command"></param>
    /// <typeparam name="TCommand"></typeparam>
    /// <returns><see cref="CommandResult"/></returns>
     public Task<CommandResult> SendCommand<TCommand>(TCommand command) where TCommand : Command;

    /// <summary>
    /// Dispatch a command to the bus
    /// </summary>
    /// <param name="command"></param>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <returns>A specific type of <see cref="CommandResult"/></returns>
     public Task<TResult> SendCommand<TCommand, TResult>(TCommand command) 
         where TCommand : Command<TResult>
          where TResult : CommandResult;
}