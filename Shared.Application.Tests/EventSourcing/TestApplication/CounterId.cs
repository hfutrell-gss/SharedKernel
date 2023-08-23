using Shared.Abstractions.Kernel;

namespace Shared.Application.Tests.EventSourcing.TestApplication;

public record CounterId(Guid Id) : AggregateRootId<Guid>(Id);