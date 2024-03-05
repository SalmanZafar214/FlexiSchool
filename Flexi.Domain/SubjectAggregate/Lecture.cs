using Flexi.Domain.LectureTheaterAggregate.ValueObjects;
using Flexi.Domain.SubjectAggregate.ValueObjects;

namespace Flexi.Domain.SubjectAggregate;

public class Lecture
{
    public LectureId Id { get; set; }
    public LectureTheaterId TheaterId { get; set; }

    public DayOfWeek DayOfWeek { get; private set; }

    public DateTime TimeOfDay { get; private set; }
    public int Duration { get; private set; }

    public Lecture(LectureId id, LectureTheaterId theaterId,
        DayOfWeek dayOfWeek,
        DateTime timeOfDay,
        int duration)
    {
        Id = id;
        TheaterId = theaterId;
        DayOfWeek = dayOfWeek;
        TimeOfDay = timeOfDay;
        Duration = duration;
    }

    //public static Lecture Make(LectureTheaterId theaterId, LectureDay day, DateTime time, int duration)
    //{
    //    Require.NotDefault(theaterId);
    //    Require.NotNull(day);

    //    return new Lecture(LectureId.Make(),
    //        theaterId,
    //        day,
    //        time,
    //        duration);
    //}
}