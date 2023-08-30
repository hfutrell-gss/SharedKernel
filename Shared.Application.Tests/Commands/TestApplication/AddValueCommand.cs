using MediatR;
using Shared.Abstractions.Commands;

namespace Shared.Application.Tests.Commands.TestApplication;

public record AddValueCommand(int Value) : Command<AddValueResult>;

public record SubtractValueCommand(int Value) : Command;