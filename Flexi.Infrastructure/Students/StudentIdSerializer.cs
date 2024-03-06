using Flexi.Domain.StudentAggregate.ValueObjects;
using Flexi.Infrastructure.Mongo;

namespace Flexi.Infrastructure.Students;

internal class StudentIdSerializer : StringSerializer<StudentId>
{
    protected override StudentId FromString(string value)
    {
        var guid = new Guid(value);
        return StudentId.Make(guid);
    }

    protected override string? ToString(StudentId? value) =>
        value?.Value.ToString();
}