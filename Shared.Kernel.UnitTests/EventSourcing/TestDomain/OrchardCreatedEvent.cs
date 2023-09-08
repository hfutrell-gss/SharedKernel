using Shared.Abstractions.EventSourcing.Writing;

namespace Shared.Kernel.UnitTests.EventSourcing.TestDomain;

public record OrchardCreatedEvent(OrchardId? Id, string? Name) : CreationEvent<OrchardId>(Id);