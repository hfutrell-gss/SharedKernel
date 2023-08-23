using Shared.Abstractions.EventSourcing.Writing;
using Shared.Kernel.EventSourcing;
using Shared.Kernel.UnitTests.EventSourcing.TestDomain;
using Xunit.Abstractions;

namespace Shared.Kernel.UnitTests.EventSourcing;

public class EventSourcingAggregateRootTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private OrchardId _orchardId;
    private string? _orchardName;
    private Orchard _orchard;

    [Fact]
    void
        an_aggregates_id_can_be_rehydrated_using_its_creation_event
        ()
    {
        var e = GetCreateOrchardEvent();
        var orchard = Orchard.FromHistory(new[] {e});
        Assert.Equal(_orchardId, orchard.Id);
    }

    [Fact]
    void
        changes_on_aggregate_creation_are_rehydrated_using_its_creation_event
        ()
    {
         var e = GetCreateOrchardEvent();
         var orchard = Orchard.FromHistory(new[] {e});
         Assert.Equal(_orchardName, orchard.Name);       
    }
    
    [Fact]
    void
        creation_events_not_from_internal_cannot_be_added
        ()
    {
        var id = new OrchardId(Guid.NewGuid());
        var creation = new OrchardCreatedEvent(id, "smith's orchard");
        
        Assert.Throws<InvalidOperationException>(() =>  Orchard.FromHistory(new ChangeEvent[]{creation}));
    }
    
    [Fact]
    void
        in_order_subsequent_change_events_are_applied_in_order
        ()
    {
        var creation = GetCreateOrchardEvent();
        var tree1 = GetTreeAddedEvent() as TreeAddedEvent;
        var tree2 = GetTreeAddedEvent() as TreeAddedEvent;
    
        var orchard = Orchard.FromHistory(new ChangeEvent[] {creation, tree1!, tree2!});
            
        Assert.Equal(tree1!.TreeType, orchard.Trees[0].TreeType);
        Assert.Equal(tree2!.TreeType, orchard.Trees[1].TreeType);
    }
     
    [Fact]
    void
        out_of_order_subsequent_change_events_are_applied_in_order
        ()
    {
        var creation = GetCreateOrchardEvent();
        var tree1 = GetTreeAddedEvent() as TreeAddedEvent;
        var tree2 = GetTreeAddedEvent() as TreeAddedEvent;

        var orchard = Orchard.FromHistory(new ChangeEvent[] {creation, tree2!, tree1!});
        
        Assert.Equal(tree1!.TreeType, orchard.Trees[0].TreeType);
        Assert.Equal(tree2!.TreeType, orchard.Trees[1].TreeType);
    }
    
    [Fact]
    void
        if_events_are_missing_then_throws_exception
        ()
    {
        var creation = GetCreateOrchardEvent();
        var tree1 = GetTreeAddedEvent() as TreeAddedEvent;
        var tree2 = GetTreeAddedEvent() as TreeAddedEvent; // left out of history
        var tree3 = GetTreeAddedEvent() as TreeAddedEvent;

        Assert.Throws<InvalidOperationException>(() => Orchard.FromHistory(new ChangeEvent[] {creation, tree1!, tree3!}));
    }

    [Fact]
    void
        if_no_creation_event_then_fails
        ()
    {
        var creation = GetCreateOrchardEvent();
        var tree1 = GetTreeAddedEvent() as TreeAddedEvent;
        var tree2 = GetTreeAddedEvent() as TreeAddedEvent; // left out of history
        var tree3 = GetTreeAddedEvent() as TreeAddedEvent;
    
        Assert.Throws<InvalidOperationException>(() => Orchard.FromHistory(new ChangeEvent[] {tree1!, tree2!, tree3!}));
    }
    
    [Fact]
    void
        if_fake_creation_event_then_fails
        ()
    {
        var creation = GetCreateOrchardEvent();
        var tree1 = new TreeAddedEvent(creation.Id, new TreeId(Guid.NewGuid()), "apple");
        
        Assert.Throws<InvalidOperationException>(() => Orchard.FromHistory(new ChangeEvent[] {tree1}));
    }

    [Fact]
    void
        more_than_one_creation_event_cannot_be_created
        ()
    {
        var creation = GetCreateOrchardEvent();
        var tree1 = GetTreeAddedEvent() as TreeAddedEvent;
        var tree2 = GetTreeAddedEvent() as TreeAddedEvent; // left out of history
        var tree3 = GetTreeAddedEvent() as TreeAddedEvent;
        Assert.Null(GetBadCreateOrchardEvent());
    }

    [Fact]
    void
        creation_events_have_aggregate_id
        ()
    {
        var creation = GetCreateOrchardEvent();
        
        Assert.Equal(_orchardId, creation.AggregateId);
    }

    [Fact]
    void
        change_events_have_aggregate_id
        ()
    {
        var creation = GetCreateOrchardEvent();
        var tree = GetTreeAddedEvent();
            
        Assert.Equal(_orchardId, tree.AggregateId);
    }

    [Fact]
    void
        rehydrating_an_aggregate_does_not_keep_events
        ()
    {
        var creation = GetCreateOrchardEvent();
        var tree = GetTreeAddedEvent();

        var orchard2 = Orchard.FromHistory(new[] {creation, tree});
        Assert.Equal(0, orchard2.EventSourcingEvents.Count);
    }

    [Fact]
    void
        rehydrating_an_aggregate_without_events_is_handled
        ()
    {
        Assert.Throws<InvalidOperationException>(() => Orchard.FromHistory(new ChangeEvent []{}));
    }

    [Fact]
    void
        can_do_binding
        ()
    {
        Assert.True(BindCreation().WasSuccessful);
    }

    [Fact]
    void
        an_exception_in_binding_is_handled
        ()
    {
        Assert.False(BindCreation().Bind(o => o.ThrowException()).WasSuccessful);
    }
    
    [Fact]
    void
        an_exception_in_binding_returns_the_message
        ()
    {
        Assert.NotEmpty(BindCreation().Bind(o => o.ThrowException()).FailureReasons);
    }

    [Fact]
    void
        can_throw_the_exception_from_the_result_when_exception
        ()
    {
        Assert.ThrowsAsync<Exception>(() => throw BindCreation().Bind(o => o.ThrowException()).Exception);
    }

    [Fact]
    void
        can_do_mapping
        ()
    {
        var orchard = BindCreation().ResultValue;
        var result = BindCreation().Map(o => o.Name);
        
        Assert.Equal(orchard.Name, result.ResultValue);
    }
    
    [Fact]
    void
        exception_in_mapping_returns_exception_message
        ()
    {
        var result = BindCreation().Map<string>(o => throw new Exception("Ruh roh"));
            
        Assert.Equal("Ruh roh", result.FailureReasons.First());
    }

    [Fact]
    void
        exception_in_mapping_is_not_successful
        ()
    {
        var result = BindCreation().Map<string>(o => throw new Exception("Ruh roh"));
            
        Assert.False(result.WasSuccessful);
    }

    [Fact]
    void
        invalid_binding_works_without_exceptions
        ()
    {
        Assert.False(InvalidBindCreation().WasSuccessful);
    }

    [Fact]
    void
        invalid_binding_reports_invalidations
        ()
    {
        Assert.NotEmpty(InvalidBindCreation().FailureReasons);
    }

    public EventSourcingAggregateRootTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

   
    private CreationEvent<OrchardId> GetCreateOrchardEvent()
    {
        _orchardName = $"smith's orchard";
        return GetCreateOrchardEvent(_orchardName);
    }

    private CreationEvent<OrchardId> GetBadCreateOrchardEvent()
    {
        _orchard.DoIncorrectCreation("Bad orchard");
        var e = _orchard.Events.Last() as CreationEvent<OrchardId>;

        return e;
    }
    
    private CreationEvent<OrchardId> GetCreateOrchardEvent(string name)
    {
        _orchardId = new OrchardId(Guid.NewGuid());
        _testOutputHelper.WriteLine($"{_orchardId} created");
        _orchard = new Orchard(_orchardId, name);
        _testOutputHelper.WriteLine("Orchard created");
        _testOutputHelper.WriteLine($"{name} added");

        var e = _orchard.Events.Last() as CreationEvent<OrchardId>;
        if (e is null)
        {
            Assert.Fail("Creation event failed");
        }
        
        return e;
    }
 
    private ChangeEvent GetTreeAddedEvent()
    {
        return GetTreeAddedEvent(_orchard);
    }

    private ChangeEvent GetTreeAddedEvent(Orchard orchard)
    {
        orchard
            .AddTree(Guid.NewGuid().ToString())
            ;

        var e = orchard.Events.Last() as TreeAddedEvent;
        if (e is null)
        {
            Assert.Fail("Adding tree event failed");
        }
        
        return e;
    }

    private ChangeResult<Orchard> BindCreation()
    {
        return Orchard.Create()
            .AddTree("maple")
            .Bind(o => o.AddTree("orange"))
            .Bind(o => o.AddTree("apple"));
    }

    private ChangeResult<Orchard> InvalidBindCreation()
    {
        return Orchard.Create()
            .AddTree("maple")
            .Bind(o => o.AddTree("orange"))
            .Bind(o => o.AddTree("invalid"));
    }

}