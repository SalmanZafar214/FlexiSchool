using Flexi.Domain.Core.Guard;
using Flexi.Domain.Core.ValueObjects;

namespace Flexi.Domain.StudentAggregate.ValueObjects;

public sealed class StudentId : PrimitiveValueObject<Guid>
{
    private StudentId(Guid value) : base(value)
    {
    }

    public static StudentId Make(Guid value)
    {
        Require.NotDefault(value);
        return new StudentId(value);
    }

    public static StudentId MakeNew() => new(Guid.NewGuid());
}