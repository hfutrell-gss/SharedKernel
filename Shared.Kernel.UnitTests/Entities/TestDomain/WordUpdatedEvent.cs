using Shared.Abstractions.Kernel;

namespace Shared.Kernel.UnitTests.Entities.TestDomain;

public record WordUpdatedEvent(string Word) : DomainEvent;