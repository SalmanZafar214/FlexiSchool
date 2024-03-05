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

    public List<Lecture>? Lectures { get; private set; }

    private LectureTheater(LectureTheaterId id,
        string name,
        int capacity,
        List<Lecture>? lectures,
        UserId createdBy,
        DateTime createdOn,
        UserId modifiedBy,
        DateTime modifiedOn)
        : base(id, createdBy, createdOn, modifiedBy, modifiedOn)
    {
        Name = name;
        Capacity = capacity;
        Lectures = lectures;
    }

    public static LectureTheater Make(string name, int capacity, List<Lecture>? lectures, UserId createdBy, UserId modifiedBy)
    {
        Require.NotNullOrEmpty(name);

        return new LectureTheater(LectureTheaterId.MakeNew(),
            name,
            capacity,
            lectures ?? new List<Lecture>(),
            createdBy,
            DateTime.Now,
            modifiedBy,
            DateTime.Now);
    }
}