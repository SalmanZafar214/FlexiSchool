using Flexi.Infrastructure.Mongo;
using DayOfWeek = Flexi.Domain.SubjectAggregate.DayOfWeek;

namespace Flexi.Infrastructure.Subjects;

public sealed class DayOfWeekSerializer : StringSerializer<DayOfWeek>
{
    protected override DayOfWeek FromString(string value)
    {
        return DayOfWeek.FromName(value);
    }

    protected override string ToString(DayOfWeek? value)
    {
        return value?.Value ?? "";
    }
}