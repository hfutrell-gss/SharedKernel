using Shared.Abstractions.Commands;
using Shared.Abstractions.Kernel;
using Shared.Abstractions.Results;

namespace Shared.Application.Tests.Commands.TestApplication;

public class AddValueCommandHandler : ICommandHandler<AddValueCommand, AddValueResult>
{
    public Task<Result<AddValueResult>> Handle(AddValueCommand request, CancellationToken cancellationToken)
    {
        if (request.Value % 2 == 0)
        {
            return Task.FromResult(Result<AddValueResult>.Success(new AddValueResult(request.Value + 1)));
        }

        return Task.FromResult(Result<AddValueResult>.Fail("No good"));
    }
}

public class SubtractValueCommandHandler : ICommandHandler<SubtractValueCommand>
{
    public Task<Result> Handle(SubtractValueCommand request, CancellationToken cancellationToken)
    {
        if (request.Value % 2 == 0)
        {
            return Task.FromResult(Result.Success());
        }
        
        return Task.FromResult(Result.Fail("value was odd"));
    }
}