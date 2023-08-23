namespace Shared.Abstractions.EventSourcing.Writing;

public sealed record EventSequenceNumber(long Number) : IComparable<EventSequenceNumber>
{
    public static EventSequenceNumber operator +(EventSequenceNumber a, long b) => new(a.Number + b);

    public static bool operator ==(EventSequenceNumber a, int b) => a.Number == b;
    
    public static bool operator !=(EventSequenceNumber a, int b) => !(a == b);

    public int CompareTo(EventSequenceNumber? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return Number.CompareTo(other.Number);
    }
}