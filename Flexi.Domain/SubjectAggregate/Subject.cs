using Flexi.Domain.Core.Aggregate;
using Flexi.Domain.Core.Guard;
using Flexi.Domain.Core.ValueObjects;
using Flexi.Domain.StudentAggregate;
using Flexi.Domain.SubjectAggregate.Events;
using Flexi.Domain.SubjectAggregate.ValueObjects;

namespace Flexi.Domain.SubjectAggregate;

public class Subject : AggregateRoot<SubjectId>
{
    public string Name { get; private set; }

    public List<Lecture> Lectures { get; private set; }

    public List<Student> StudentsEnrolled { get; set; }

    private Subject(SubjectId id,
        string name,
        List<Lecture>? lectures,
        UserId createdBy,
        DateTime createdOn,
        UserId modifiedBy,
        DateTime modifiedOn)
        : base(id, createdBy, createdOn, modifiedBy, modifiedOn)
    {
        Name = name;
        Lectures = lectures ?? new List<Lecture>();

        AddEvent(new SubjectCreated(id, name));
    }

    public static Subject Make(string name, List<Lecture>? lectures, UserId createdBy, UserId modifiedBy)
    {
        Require.NotNullOrEmpty(name);
        Require.NotNullOrEmpty(lectures);

        return new Subject(SubjectId.Make(),
            name,
            lectures,
            createdBy,
            DateTime.Now,
            modifiedBy,
            DateTime.Now);
    }

    public void AddLecture(Lecture lecture)
    {
        Lectures.Add(lecture);

        AddEvent(new LectureAddedToSubject(lecture.Id, Name, Id));
    }

    public void RemoveLecture(Lecture lecture)
    {
        if (Lectures.Any())
        {
            Lectures.Remove(lecture);
        }
    }
}