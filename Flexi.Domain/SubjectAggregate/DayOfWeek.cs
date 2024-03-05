using Flexi.Domain.Core.Exceptions;
using Flexi.Domain.Core.ValueObjects;

namespace Flexi.Domain.SubjectAggregate;

public class DayOfWeek : PrimitiveValueObject<string>
{
    public int Id { get; }

    public static DayOfWeek Monday => new("Monday", (int)LectureDayValues.Monday);
    public static DayOfWeek Tuesday => new("Tuesday", (int)LectureDayValues.Tuesday);
    public static DayOfWeek Wednesday => new("Wednesday", (int)LectureDayValues.Wednesday);
    public static DayOfWeek Thursday => new("Thursday", (int)LectureDayValues.Thursday);

    public DayOfWeek(string value, int id) : base(value)
    {
        Id = id;
    }

    private static IEnumerable<DayOfWeek> AllFormTypes => new[]
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday
    };

    public static DayOfWeek FromName(string name)
    {
        var matchingFormTypes = AllFormTypes
            .SingleOrDefault(r => string.Equals(r.Value, name, StringComparison.OrdinalIgnoreCase));

        return matchingFormTypes ?? throw new NotFoundException(nameof(DayOfWeek), name);
    }
}

public enum LectureDayValues
{
    Monday = 1,
    Tuesday = 2,
    Wednesday = 3,
    Thursday = 4
}