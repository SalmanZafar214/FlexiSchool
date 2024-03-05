using Flexi.Domain.Core.Guard;
using Flexi.Domain.Core.ValueObjects;

namespace Flexi.Domain.LectureTheaterAggregate.ValueObjects;

public sealed class LectureTheaterId : PrimitiveValueObject<Guid>
{
    private LectureTheaterId(Guid value) : base(value)
    {
    }

    public static LectureTheaterId Make(Guid value)
    {
        Require.NotDefault(value);
        return new LectureTheaterId(value);
    }

    public static LectureTheaterId MakeNew() => new(Guid.NewGuid());
}