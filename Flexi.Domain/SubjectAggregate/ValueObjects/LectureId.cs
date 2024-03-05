using Flexi.Domain.Core.Guard;
using Flexi.Domain.Core.ValueObjects;

namespace Flexi.Domain.SubjectAggregate.ValueObjects;

public class LectureId : PrimitiveValueObject<Guid>
{
    private LectureId(Guid value) : base(value)
    {
    }

    public static LectureId Make(Guid value)
    {
        Require.NotDefault(value);
        return new LectureId(value);
    }

    public static LectureId Make() => new(Guid.NewGuid());
}