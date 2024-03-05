namespace Flexi.Application;

internal static class Endpoints
{
    public const string LectureTheater = "lecture-theater";

    public const string Subject = "subject";

    public static class Subjects
    {
        public const string Subject = "subject";
        public const string Lecture = $"subject/{{SubjectId}}/lecture";
    }
}