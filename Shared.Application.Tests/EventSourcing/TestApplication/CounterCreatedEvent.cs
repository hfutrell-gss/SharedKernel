using Shared.Abstractions.EventSourcing.Writing;

namespace Shared.Application.Tests.EventSourcing.TestApplication;

internal record CounterCreatedEvent(CounterId Id) : CreationEvent<CounterId>(Id);