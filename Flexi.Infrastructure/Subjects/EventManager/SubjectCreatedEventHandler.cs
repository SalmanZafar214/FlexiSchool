using Flexi.Domain.Core.Events;
using Flexi.Domain.SubjectAggregate;
using Flexi.Domain.SubjectAggregate.Events;
using Microsoft.Extensions.Logging;

namespace Flexi.Infrastructure.Subjects.EventManager;

internal class SubjectCreatedEventHandler : IDomainEventHandler<Subject>
{
    private readonly ILogger<SubjectCreatedEventHandler> logger;

    public SubjectCreatedEventHandler(ILogger<SubjectCreatedEventHandler> logger)
    {
        this.logger = logger;
    }

    public Task HandleEvent(Subject entity)
    {
        var isSubjectCreated = entity.Events.Any(e => e is SubjectCreated);

        if (isSubjectCreated)
        {
            logger.LogInformation("Published Subject Created message");
        }

        return Task.CompletedTask;
    }
}