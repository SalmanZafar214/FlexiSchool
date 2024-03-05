namespace Flexi.Domain.Core.ValueObjects;

public sealed class UserId : PrimitiveValueObject<Guid>, IEquatable<UserId>
{
    private UserId(Guid value) : base(value)
    {
    }

    public static UserId Make(Guid value)
    {
        if (value == default)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return new UserId(value);
    }

    public static UserId Make() => new(Guid.NewGuid());

    public bool Equals(UserId? other)
    {
        throw new NotImplementedException();
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((UserId)obj);
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }
}