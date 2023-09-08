using Shared.Abstractions.EventSourcing.Writing;

namespace Shared.Kernel.UnitTests.EventSourcing.TestDomain;

public record TreeAddedEvent(OrchardId Id, TreeId TreeId, string TreeType) : ChangeEvent(Id);