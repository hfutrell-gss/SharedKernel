namespace Shared.Application.Tests.Queries.TestApplication;

public class ThingStore
{
    public int GetThing(int thingToGet)
    {
        return thingToGet + 1;
    }
}