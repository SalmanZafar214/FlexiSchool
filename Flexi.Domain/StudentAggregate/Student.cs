using Flexi.Domain.Core.Aggregate;
using Flexi.Domain.Core.Guard;
using Flexi.Domain.Core.ValueObjects;
using Flexi.Domain.StudentAggregate.Events;
using Flexi.Domain.StudentAggregate.ValueObjects;

namespace Flexi.Domain.StudentAggregate;

public class Student : AggregateRoot<StudentId>
{
    public string Name { get; private set; }

    private Student(StudentId id, string name, UserId createdBy, DateTime createdOn, UserId modifiedBy, DateTime modifiedOn)
        : base(id, createdBy, createdOn, modifiedBy, modifiedOn)
    {
        Name = name;

        AddEvent(new StudentEnrolled(id, name));
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

}