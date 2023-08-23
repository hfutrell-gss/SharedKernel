using Shared.Kernel.Entities;

namespace Shared.Kernel.UnitTests.Entities.TestDomain;

internal class WordAggregate : AggregateRoot<WordAggregateId, Guid>
{
    public WordAggregate(WordAggregateId id) : base(id)
    {
    }
 
    public void UpdateWord(string word)
    {
        RegisterDomainEvent(new WordUpdatedEvent(word));
    }

    public void AnnounceNumber(int i)
    {
        var entity = new NumberEntity();
        entity.UpdateNumber(i);
        RegisterDomainEvent(new NumberUpdatedEvent(i));
    }
}