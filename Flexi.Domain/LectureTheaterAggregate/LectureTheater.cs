using Flexi.Domain.Core.Aggregate;
using Flexi.Domain.Core.Guard;
using Flexi.Domain.Core.ValueObjects;
using Flexi.Domain.LectureTheaterAggregate.ValueObjects;
using Flexi.Domain.SubjectAggregate;

namespace Flexi.Domain.LectureTheaterAggregate;

public class LectureTheater : AggregateRoot<LectureTheaterId>
{
    public string Name { get; private set; }

    public int Capacity { get; private set; }

    private LectureTheater(LectureTheaterId id,
        string name,
        int capacity,
        UserId createdBy,
        DateTime createdOn,
        UserId modifiedBy,
        DateTime modifiedOn)
        : base(id, createdBy, createdOn, modifiedBy, modifiedOn)
    {
        Name = name;
        Capacity = capacity;
    }

    public static LectureTheater Make(string name, int capacity, UserId createdBy, UserId modifiedBy)
    {
        Require.NotNullOrEmpty(name);

        return new LectureTheater(LectureTheaterId.MakeNew(),
            name,
            capacity,
            createdBy,
            DateTime.Now,
            modifiedBy,
            DateTime.Now);
    }
}