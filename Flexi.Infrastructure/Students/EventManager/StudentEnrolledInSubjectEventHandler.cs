using Flexi.Domain.Core.Events;
using Flexi.Domain.StudentAggregate;
using Flexi.Domain.StudentAggregate.Events;
using Flexi.Domain.SubjectAggregate.Events;
using Microsoft.Extensions.Logging;

namespace Flexi.Infrastructure.Students.EventManager;

internal class StudentEnrolledInSubjectEventHandler : IDomainEventHandler<Student>
{
    private readonly ILogger<StudentEnrolledInSubjectEventHandler> logger;

    public StudentEnrolledInSubjectEventHandler(ILogger<StudentEnrolledInSubjectEventHandler> logger)
    {
        this.logger = logger;
    }

    public Task HandleEvent(Student entity)
    {
        var isLectureAddedToSubject = entity.Events.Any(e => e is StudentEnrolledInSubject);

        if (isLectureAddedToSubject)
        {
            //Command should be raised to MSMQ that will be consumed by EmailService (Notification Service)
            logger.LogInformation("Published Student Enrolled message");
        }
        return Task.CompletedTask;
    }
}