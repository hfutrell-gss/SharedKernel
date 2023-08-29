using Newtonsoft.Json;

namespace Shared.Abstractions.Events;

/// <summary>
/// The type of an event
/// </summary>
public record EventType
{
    [JsonProperty]
    private string Value { get; }

    [JsonConstructor]
    private EventType(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Implicit conversion to a <see cref="string"/>
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public static implicit operator string(EventType e) => e.Value;
    
    /// <summary>
    /// Implicit conversion from a <see cref="string"/> to an <see cref="EventType"/>
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static implicit operator EventType(string s) => new(s);
    
    /// <summary>
    /// Implicit conversion to a valid <see cref="EventType"/> from a <see cref="Type"/>
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static implicit operator EventType(Type t)
    {
        var typeName = t.Name;
        if (!typeName.EndsWith("Event"))
            throw new ArgumentException($"The type name {typeName} is invalid for an event. Event names must end with Event");

        return string.Join("", typeName.SkipLast(5));
    }
}