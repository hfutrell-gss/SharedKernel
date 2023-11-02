using Shared.Abstractions.EventSourcing.Writing;
using Shared.Abstractions.Kernel;
using Shared.Abstractions.Results;
using Shared.Kernel.Entities;

namespace Shared.Kernel.EventSourcing;

/// <summary>
/// Contains event sourcing logic and necessary constraints
/// for implementing event sourced aggregates
/// </summary>
/// <typeparam name="TRoot">
/// The type of <see cref="AggregateRoot{TId,TIdType}"/> encapsulated in <see cref="Result"/>.
/// The intent is for this to be self-referential for method chaining.
/// </typeparam>
/// <typeparam name="TId"><see cref="AggregateRootId{TId}"/> of the aggregate type type. Must be a type of <see cref="Guid"/></typeparam>
/// <example>
/// <code>
/// <![CDATA[
/// public class Thing : EventSourcedAggregateRoot<Thing, ThingId>
/// {
///     // constructors should be private
///     private Thing() : this(new ThingId())
///     {
///        // there should be no logic in here other than raising the event
///        TryDoChange(new ThingCreatedEvent(Id));
///     }
///
///     // constructors should be private
///     private Thing(ThingId id) : base(id)
///     {
///         // simply register the handlers and this will be all groovy
///         RegisterChangeHandler<ThingCreatedEvent>(ThingCreatedHandler);
///         RegisterChangeHandler<ThingNumberChangedEvent>(ThingNumberChangedHandler);     
///     }
///
///     public Result<Thing> ThingCreatedHandler() ...
/// 
///     public Result<Thing> ThingNumberChangedHandler() ...
///
///     // Objects should be created with static factory methods
///     public static Result<Thing> Create()
///     {
///         return new Thing();
///     }
///
///     public static Thing FromHistory(IEnumerable<ChangeEvent> history)
///     {
///         return Rehydrate(thingId => new Thing(thingId), history);
///     }
/// }
///
/// 
///  // chaining can be done with binding
/// var thing = Thing.Create()
///     .Bind(t => t.ChangeNumber)
///     )))
///  
/// ]]>
/// </code>
/// </example>
public abstract class EventSourcedAggregateRoot<TRoot, TId> : AggregateRoot<TId, Guid>, IEventSourcedAggregateRoot<TId>
    where TId : AggregateRootId<Guid>
    where TRoot : EventSourcedAggregateRoot<TRoot, TId>
{
    /// <summary>
    /// Events used for event sourcing
    /// </summary>
    public IReadOnlyCollection<ChangeEvent> EventSourcingEvents => Events()
        .Select(e => e as ChangeEvent)
        .Where(e => e is not null)
        .ToList().AsReadOnly()!;
    
    private readonly Dictionary<Type, Func<ChangeEvent, Result<TRoot>>> _changeEventHandlers = new();
    
    /// <summary>
    /// It is not reliable to depend on dates or times in event sourcing.
    /// The sequence number is used to enforce ordering.
    /// Using optimistic concurrency.
    /// </summary>
    private EventSequenceNumber _currentEventSequenceNumber = new(0);


    /// <summary>
    /// The best pattern is to have a parameterless constructor
    /// and an Id constructor that create the aggregate. Chain them
    /// such that the new() constructor create the CreatedEvent
    /// and the other registers the change handlers -- including for
    /// the creation event
    /// </summary>
    /// <param name="id">The item's unique id. Should be convertible to a <see cref="Guid"/></param>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// public class Thing : EventSourcedAggregateRoot<Thing, ThingId>
    /// {
    ///     private Thing(ThingId id) : base(id)
    ///     {
    ///         RegisterChangeHandler<ThingCreatedEvent>(ThingCreatedHandler);
    ///         RegisterChangeHandler<ThingNumberChangedEvent>(ThingNumberChangedHandler);     
    ///     }
    ///
    ///     
    ///     public Thing() : this(new ThingId())
    ///     {
    ///         DoChange(new ThingCreatedEvent(Id));
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    protected EventSourcedAggregateRoot(TId id) : base(id)
    {
    }

    /// <summary>
    /// Register a handler to implement the changes when an event is
    /// created or sourced.
    /// The changes should already be validated at this point so
    /// the handler should try to make the requested updates to the model.
    /// <br/>
    /// The handler is invoked when <see cref="TryDoChange{TChangeEvent}"/> is called
    /// 
    /// </summary>
    /// <param name="handler">A method taking a <see cref="ChangeEvent"/> and returning a <see cref="Result"/></param>
    /// <typeparam name="TChangeEvent">The <see cref="Type"/> of the <see cref="ChangeEvent"/></typeparam>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// private Orchard(Orchard id) : base(id)
    /// {
    ///     ...
    ///     RegisterChangeHandler<TreeAddedEvent>(TreeAddedHandler);     
    ///     ...
    /// }
    ///
    /// 
    /// public Result<Orchard> AddTree(string treeType)
    /// {
    ///     if (!IsValidTreeType(treeType)) return Failure("tree type not valid");
    ///
    ///     return TryDoChange(new TreeAddedEvent(Id, treeType);
    /// }
    /// 
    /// ]]>
    /// </code>
    /// </example>
    protected void RegisterChangeHandler<TChangeEvent>(Func<TChangeEvent, Result<TRoot>> handler) where TChangeEvent : ChangeEvent
    {
        _changeEventHandlers[typeof(TChangeEvent)] = @event => handler((TChangeEvent) @event);
    }
 
    /// <summary>
    /// Report a successful change
    /// </summary>
    /// <returns></returns>
    protected Result<TRoot> Success()
    {
        return Result<TRoot>.Success((TRoot)this);
    }

    /// <summary>
    /// Report a failed change and the reasons for failure
    /// </summary>
    /// <param name="failureReasons"></param>
    /// <returns></returns>
    protected Result<TRoot> Fail(params string[] failureReasons)
    {
        return Result<TRoot>.Fail(failureReasons);
    }      
     
    /// <summary>
    /// Perform a new change based on the event type. Done prior to processing
    /// updates
    /// </summary>
    /// <param name="event">The <see cref="ChangeEvent"/> that should be responded to</param>
    /// <typeparam name="TChangeEvent">The <see cref="Type"/> of the <see cref="ChangeEvent"/></typeparam>
    protected Result<TRoot> TryDoChange<TChangeEvent>(TChangeEvent @event) where TChangeEvent : ChangeEvent
    {
        if (_currentEventSequenceNumber.Number > 1 && @event is CreationEvent<TId>)
        {
            return Fail("Multiple creation events registered");
        }
        
        return ApplyChange(@event).Resolve(
            forSuccess: root =>
            {
                _currentEventSequenceNumber += 1;

                @event.SetSequence(_currentEventSequenceNumber);

                RegisterDomainEvent(@event);

                return Result<TRoot>.Success(root);
            },
            forFailure: failure => Result<TRoot>.Fail(failure));
    }

           
    /// <summary>
    /// Applies the registered change event handler
    /// </summary>
    /// <param name="event"></param>
    /// <typeparam name="TChangeEvent"></typeparam>
    private Result<TRoot> ApplyChange<TChangeEvent>(TChangeEvent @event) where TChangeEvent : ChangeEvent
    {
        if (!_changeEventHandlers.TryGetValue(@event.GetType(), out var handler))
        {
            throw new KeyNotFoundException($"The change event handler for {@event.GetType().Name} was not registered");
        }
        return handler(@event);
    }
    
    /// <summary>
    /// Rehydrates an aggregate from a sequence of events.
    /// This returns an aggregate to the state based
    /// on changes.   
    /// </summary>
    /// <param name="typeGenerator">A function to create an instance of the type T</param>
    /// <param name="eventHistory">The events to be applied</param>
    /// <typeparam name="T">This should correspond to the aggregate being created</typeparam>
    /// <returns>A new instanced of the aggregate</returns>
    /// <exception cref="InvalidOperationException">If the events don't begin with a <see cref="Abstractions.EventSourcing.Writing.CreationEvent{T}"/></exception>
    /// <exception cref="InvalidOperationException">If there is more than one <see cref="Abstractions.EventSourcing.Writing.CreationEvent{T}"/></exception>
    /// <exception cref="InvalidOperationException">If the events are not in a strictly ordered sequence from 1..n with a step of 1</exception>
    protected static T Rehydrate<T>(Func<TId, T> typeGenerator, IEnumerable<ChangeEvent> eventHistory)
        where T : EventSourcedAggregateRoot<TRoot, TId>
    {
        var changeEvents = eventHistory.OrderBy(e => e.SequenceNumber).ToArray();

        var sequenceAnalysis = ChangeEventSequenceAnalysis(changeEvents);
        if (!sequenceAnalysis.IsValid)
        {
            throw new InvalidOperationException(string.Join("\n", sequenceAnalysis.InvalidationReasons));
        }
        
        var entity = typeGenerator(sequenceAnalysis.CreationEvent!.Id);

        foreach (var changeEvent in changeEvents)
        {
            entity.ApplyChange(changeEvent);
        }

        return entity;
    }

    /// <summary>
    /// Determines the sequence of events to be rehydrated from
    /// makes sense.
    /// </summary>
    /// <param name="changeEvents"></param>
    /// <returns></returns>
    private static (bool IsValid, List<string> InvalidationReasons, CreationEvent<TId>? CreationEvent) 
        ChangeEventSequenceAnalysis(ChangeEvent[] changeEvents)
    {
        var isValid = true;
        var invalidationReasons = new List<string>();

        if (!changeEvents.Any())
        {
            isValid = false;
            invalidationReasons.Add("There are no events to rehydrate from");
            return (isValid, invalidationReasons, null);
        }
        CreationEvent<TId>? possibleCreationEvent = changeEvents.First() as CreationEvent<TId>;

        if (possibleCreationEvent is null)
        {
            isValid = false;
            invalidationReasons.Add("The entity's creation event is missing");
        }
        
        if(possibleCreationEvent is not null &&
           possibleCreationEvent.SequenceNumber is null)
        {
            isValid = false;
            invalidationReasons.Add("The entity's creation event was not created with a valid sequence. Events must be created through aggregates");
        }

        if (possibleCreationEvent is not null &&
            possibleCreationEvent.SequenceNumber is not null &&
            possibleCreationEvent.SequenceNumber != 1)
        {
            isValid = false;
            invalidationReasons.Add("The entity's creation event is not in sequence");           
        }

        var sequence = new EventSequenceNumber(1);
        foreach (var changeEvent in changeEvents.Skip(1))
        {
            if (changeEvent is CreationEvent<TId>)
            {
                isValid = false;
                invalidationReasons.Add("The entity has multiple creation events");
            }

            if (changeEvent.SequenceNumber != sequence + 1)
            {
                isValid = false;
                invalidationReasons.Add($"A change event is missing between sequence {sequence} and {changeEvent}");
            }

            sequence += 1;
        }

        return (isValid, invalidationReasons, possibleCreationEvent);
    }

}