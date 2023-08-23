using Shared.Abstractions.Events;
using Shared.Kernel.Events;

namespace Shared.Kernel.UnitTests.Entities.TestDomain;

public record WordUpdatedEvent(string Word) : DomainEvent;