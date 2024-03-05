using Flexi.Domain.Core.Guard;
using Flexi.Domain.Core.ValueObjects;

namespace Flexi.Domain.SubjectAggregate.ValueObjects;

public class SubjectId : PrimitiveValueObject<Guid>
{
    private SubjectId(Guid value) : base(value)
    {
    }

    public static SubjectId Make(Guid value)
    {
        Require.NotDefault(value);
        return new SubjectId(value);
    }

    public static SubjectId Make() => new(Guid.NewGuid());
}