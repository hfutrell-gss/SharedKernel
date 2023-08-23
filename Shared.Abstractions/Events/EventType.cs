using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Shared.Abstractions.Events;

/// <summary>
/// The type of an event
/// </summary>
public record EventType
{
    private static readonly int MaxLength = 50;
    private static readonly Regex WhiteSpace = new("\\s");
    private static readonly Regex NonWordCharacters = new("\\W");

    [JsonProperty]
    private string Value { get; }

    [JsonConstructor]
    private EventType(string value)
    {
        if (string.IsNullOrEmpty(value)) throw new ArgumentException("Event types must have a value");
        
        if (value.Length > MaxLength) throw new ArgumentException($"Event types cannot be longer than {MaxLength} characters");
        
        if (WhiteSpace.IsMatch(value)) throw new ArgumentException("Event types cannot contain whitespace characters");
        
        if (NonWordCharacters.IsMatch(value)) throw new ArgumentException("Event types cannot contain non word characters");
        
        if (char.IsLower(value.First())) throw new ArgumentException("Event types must be pascal cased");
        
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