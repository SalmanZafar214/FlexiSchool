using Flexi.Domain.Core.Events;
using Flexi.Domain.SubjectAggregate;
using Flexi.Domain.SubjectAggregate.Events;
using Microsoft.Extensions.Logging;

namespace Flexi.Infrastructure.Subjects.EventManager
{
    internal class LectureAddedToSubjectEventHandler : IDomainEventHandler<Subject>
    {
        private readonly ILogger<LectureAddedToSubjectEventHandler> logger;

        public LectureAddedToSubjectEventHandler(ILogger<LectureAddedToSubjectEventHandler> logger)
        {
            this.logger = logger;
        }

        public Task HandleEvent(Subject entity)
        {
            var isLectureAddedToSubject = entity.Events.Any(e => e is LectureAddedToSubject);

            if (isLectureAddedToSubject)
            {
                logger.LogInformation("Published Subject Created message");
            }
            return Task.CompletedTask;
        }
    }
}