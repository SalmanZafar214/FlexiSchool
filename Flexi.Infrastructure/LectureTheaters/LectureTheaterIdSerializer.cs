using Flexi.Domain.LectureTheaterAggregate.ValueObjects;
using Flexi.Infrastructure.Mongo;

namespace Flexi.Infrastructure.LectureTheaters;

internal sealed class LectureTheaterIdSerializer : StringSerializer<LectureTheaterId>
{
    protected override LectureTheaterId FromString(string value)
    {
        var guid = new Guid(value);
        return LectureTheaterId.Make(guid);
    }

    protected override string? ToString(LectureTheaterId? value) =>
        value?.Value.ToString();
}