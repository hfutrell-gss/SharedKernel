using Shared.Abstractions.Commands;

namespace Shared.Application.Tests.Commands.TestApplication;

public record AddValueCommand(int Value) : Command<AddValueCommandResult>;

public record SubtractValueCommand(int Value) : Command;