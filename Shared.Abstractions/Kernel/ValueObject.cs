using System.Diagnostics.CodeAnalysis;

namespace Shared.Abstractions.Kernel;

/// <summary>
/// Wraps a single value object
/// </summary>
/// <param name="Value"></param>
/// <typeparam name="TObject"></typeparam>
[ExcludeFromCodeCoverage]
public abstract record ValueObject<TObject>(TObject Value)
{
    /// <summary>
    /// Implicitly convert to the object type 
    /// </summary>
    /// <param name="object"></param>
    /// <returns></returns>
    public static implicit operator TObject(ValueObject<TObject> @object) => @object.Value;
}