using Flexi.Domain.SubjectAggregate.ValueObjects;
using Flexi.Infrastructure.Mongo;

namespace Flexi.Infrastructure.Subjects;

internal sealed class LectureIdSerializer : StringSerializer<LectureId>
{
    protected override LectureId FromString(string value)
    {
        var guid = new Guid(value);
        return LectureId.Make(guid);
    }

    protected override string? ToString(LectureId? value) =>
        value?.Value.ToString();
}