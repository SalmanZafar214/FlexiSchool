using Flexi.Domain.Core.Aggregate;
using Flexi.Domain.Core.Exceptions;
using Flexi.Domain.Core.Guard;
using Flexi.Domain.Core.ValueObjects;
using Flexi.Domain.StudentAggregate;
using Flexi.Domain.StudentAggregate.Events;
using Flexi.Domain.StudentAggregate.ValueObjects;
using Flexi.Domain.SubjectAggregate.Events;
using Flexi.Domain.SubjectAggregate.ValueObjects;

namespace Flexi.Domain.SubjectAggregate;

public record SubjectStudent(string Name, StudentId Id);

public class Subject : AggregateRoot<SubjectId>
{
    public string Name { get; private set; }

    public List<Lecture> Lectures { get; private set; }

    public List<SubjectStudent> Students { get; private set; } = new();

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
        var lectureAlreadyExitsOnSameDay = Lectures.Exists(l => l.DayOfWeek.Equals(lecture.DayOfWeek)
                                                                && l.TheaterId.Equals(lecture.TheaterId));

        if (lectureAlreadyExitsOnSameDay)
        {
            throw new AlreadyExistsException(lecture.DayOfWeek.ToString(), nameof(Subject), $"Lecture already exists on Same day");
        }

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

    public void EnrollInSubject(Student student)
    {
        if (CheckStudentIsAlreadyEnrolled(student))
        {
            throw new AlreadyExistsException(Id.ToString(), nameof(Subject), "Student is already enrolled in the subject");
        }

        var totalWeeklyHours = CalculateTotalWeeklyLectureHours();
        if (totalWeeklyHours > 10)
        {
            throw new InvalidOperationException("Student cannot enroll in subject, exceeds weekly lecture hours limit.");
        }

        Students.Add(new SubjectStudent(student.Name, student.Id));

        AddEvent(new StudentEnrolledInSubject(student.Id, student.Name, Id, Name));
    }

    private int CalculateTotalWeeklyLectureHours()
    {
        return Lectures.Sum(lecture => lecture.Duration);
    }

    private bool CheckStudentIsAlreadyEnrolled(Student student)
    {
        var result = Students.Exists(s => s.Id.Equals(student.Id));
        return result;
    }
}