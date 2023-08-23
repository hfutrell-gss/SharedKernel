using Shared.Abstractions.Queries;

namespace Shared.Application.Tests.Queries.TestApplication;

public record ThingQuery(int ThingToGet) : Query<ThingQueryResult>;