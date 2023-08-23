using Shared.Abstractions.Commands;

namespace Shared.Application.Tests.Commands.TestApplication;

public sealed record AddValueCommandResult : CommandResult<AddValueCommandResult, int>;