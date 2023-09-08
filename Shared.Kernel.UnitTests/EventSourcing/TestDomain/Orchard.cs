using Shared.Abstractions.EventSourcing.Writing;
using Shared.Abstractions.Results;
using Shared.Kernel.EventSourcing;

namespace Shared.Kernel.UnitTests.EventSourcing.TestDomain;

public class Orchard : EventSourcedAggregateRoot<Orchard, OrchardId>
{
    public List<Tree> Trees { get; } = new();
    public string? Name { get; private set; }


    public Orchard() : this(new OrchardId(Guid.NewGuid()))
    {
        TryDoChange(new OrchardCreatedEvent(Id, ""));
    }
    
    public Orchard(OrchardId? id, string name) : this(id)
    {
        TryDoChange(new OrchardCreatedEvent(id, name));
    }

    private Orchard(OrchardId? id) : base(id!)
    {
        RegisterChangeHandler<OrchardCreatedEvent>(CreateOrchard);
        RegisterChangeHandler<TreeAddedEvent>(AddTree);
    }
    
    private Result<Orchard> CreateOrchard(OrchardCreatedEvent e)
    {
        Name = e.Name;

        return Success();
    }

    public Result<Orchard> AddTree(string treeType)
    {
        if ("invalid".Equals(treeType)) return Fail("tree type cannot be invalid");
        
        var e = new TreeAddedEvent(Id, new TreeId(Guid.NewGuid()), treeType);
        
        return TryDoChange(e);
    }

    public Result<Orchard> ThrowException()
    {
        throw new InvalidCastException("Something is borked");
    }

    // This is bad to do
    public void DoIncorrectCreation(string name)
    {
        TryDoChange(new OrchardCreatedEvent(Id, name));
    }
    
    private Result<Orchard> AddTree(TreeAddedEvent obj)
    {
        Trees.Add(new Tree(obj.TreeId, obj.TreeType));
        
        return Success();
    }

    public static Orchard FromHistory(IEnumerable<ChangeEvent> events)
    {
        return Rehydrate(id => new Orchard(id), events);
    }

    public static Orchard Create()
    {
        return new Orchard();
    }
}