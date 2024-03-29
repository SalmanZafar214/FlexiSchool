﻿namespace Flexi.Application;

internal static class Endpoints
{
    public const string LectureTheater = "lecture-theater";

    public static class Subjects
    {
        public const string Subject = "subject";
        public const string Lecture = $"subject/{{SubjectId}}/lecture";
        public const string Enroll = $"subject/{{SubjectId}}/Enroll";
    }

    public static class Students
    {
        public const string Student = "student";
        public const string StudentById = $"student/{{StudentId}}";
        public const string Enroll = $"student/{{StudentId}}/Enroll";
    }
}