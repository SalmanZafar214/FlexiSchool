using Flexi.Domain.Core.Aggregate;
using Flexi.Domain.Core.Guard;
using Flexi.Domain.Core.ValueObjects;
using Flexi.Domain.StudentAggregate.Events;
using Flexi.Domain.StudentAggregate.ValueObjects;
using Flexi.Domain.SubjectAggregate;

namespace Flexi.Domain.StudentAggregate;

public class Student : AggregateRoot<StudentId>
{
    public string Name { get; private set; }

    public List<Subject>? Subjects { get; private set; }

    private Student(StudentId id, string name, UserId createdBy, DateTime createdOn, UserId modifiedBy, DateTime modifiedOn)
        : base(id, createdBy, createdOn, modifiedBy, modifiedOn)
    {
        Name = name;
    }

    public static Student Make(string name, UserId createdBy, UserId modifiedBy)
    {
        Require.NotNullOrEmpty(name);

        return new Student(
            id: StudentId.MakeNew(),
            name: name,
            createdBy: createdBy,
            createdOn: DateTime.Now,
            modifiedBy: modifiedBy,
            modifiedOn: DateTime.Now);
    }

    public void EnrollInSubject(Subject subject)
    {
        var totalWeeklyHours = CalculateTotalWeeklyLectureHours(subject);
        if (totalWeeklyHours > 10)
        {
            throw new InvalidOperationException("Student cannot enroll in subject, exceeds weekly lecture hours limit.");
        }

        Subjects ??= new List<Subject>();
        Subjects.Add(subject);

        AddEvent(new StudentEnrolledInSubject(Id, Name, subject.Id, subject.Name));
    }

    private int CalculateTotalWeeklyLectureHours(Subject subject)
    {
        return subject.Lectures.Sum(lecture => lecture.Duration);
    }
}