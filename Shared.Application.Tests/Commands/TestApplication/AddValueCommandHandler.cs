using Shared.Abstractions.Commands;

namespace Shared.Application.Tests.Commands.TestApplication;

public class AddValueCommandHandler : ICommandHandler<AddValueCommand, AddValueCommandResult>
{
    public Task<AddValueCommandResult> Handle(AddValueCommand request, CancellationToken cancellationToken)
    {
        if (request.Value % 2 == 0)
        {
            return Task.FromResult(AddValueCommandResult.Success(request.Value + 1));
        }

        return Task.FromResult(AddValueCommandResult.Failure("value was odd"));
    }
}

public class SubtractValueCommandHandler : ICommandHandler<SubtractValueCommand>
{
    public Task<CommandResult> Handle(SubtractValueCommand request, CancellationToken cancellationToken)
    {
        if (request.Value % 2 == 0)
        {
            return Task.FromResult(CommandResult.Success());
        }
        
        return Task.FromResult(CommandResult.Failure("value was odd"));
    }
}