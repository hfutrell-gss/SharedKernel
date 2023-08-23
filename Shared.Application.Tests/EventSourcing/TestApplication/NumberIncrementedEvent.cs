using Shared.Abstractions.EventSourcing.Writing;

namespace Shared.Application.Tests.EventSourcing.TestApplication;

internal record NumberIncrementedEvent(Guid AggregateId) : ChangeEvent(AggregateId);