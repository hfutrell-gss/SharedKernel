using Shared.Abstractions.Commands;
using Shared.Abstractions.Kernel;

namespace Shared.Application.Tests.Commands.TestApplication;

public class AddValueCommandHandler : ICommandHandler<AddValueCommand, AddValueCommandResult>
{
    public Task<CommandResult<AddValueCommandResult>> Handle(AddValueCommand request, CancellationToken cancellationToken)
    {
        if (request.Value % 2 == 0)
        {
            return Task.FromResult(CommandResult<AddValueCommandResult>.Success(new AddValueCommandResult(request.Value + 1)));
        }

        return Task.FromResult(CommandResult<AddValueCommandResult>.Fail("No good"));
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
        
        return Task.FromResult(CommandResult.Fail("value was odd"));
    }
}