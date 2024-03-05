using Flexi.Domain.SubjectAggregate.ValueObjects;
using Flexi.Infrastructure.Mongo;

namespace Flexi.Infrastructure.Subjects;

internal class SubjectIdSerializer : StringSerializer<SubjectId>
{
    protected override SubjectId FromString(string value)
    {
        var guid = new Guid(value);
        return SubjectId.Make(guid);
    }

    protected override string? ToString(SubjectId? value) =>
        value?.Value.ToString();
}