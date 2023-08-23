using Shared.Abstractions.Queries;

namespace Shared.Application.Tests.Queries.TestApplication;

public record ThingQueryResult(int ThingGotten) : QueryResult;