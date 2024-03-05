namespace Flexi.Domain.Core.ValueObjects;

public class PrimitiveValueObject<TValue> : IEquatable<PrimitiveValueObject<TValue>>
{
    public TValue Value { get; }

    protected PrimitiveValueObject(TValue value)
    {
        Value = value;
    }

    public virtual bool Equals(PrimitiveValueObject<TValue>? other) =>
        other is not null && EqualityComparer<TValue>.Default.Equals(Value, other.Value);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((PrimitiveValueObject<TValue>)obj);
    }

    public override int GetHashCode() => EqualityComparer<TValue>.Default.GetHashCode(Value!);

    public static bool operator ==(PrimitiveValueObject<TValue>? lhs, PrimitiveValueObject<TValue> rhs)
    {
        if (lhs is null)
        {
            return rhs is null;
        }

        return lhs.Equals(rhs);
    }

    public static bool operator !=(PrimitiveValueObject<TValue> lhs, PrimitiveValueObject<TValue> rhs) => !(lhs == rhs);

    public override string ToString() => Value!.ToString() ?? string.Empty;
}