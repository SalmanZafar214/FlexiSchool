using Flexi.Domain.Core.Events;
using Flexi.Domain.SubjectAggregate;
using Microsoft.Extensions.Logging;

namespace Flexi.Infrastructure.Subjects.EventManager;

internal class SubjectEventManager : IDomainEventManager<Subject>
{
    private readonly ILogger<SubjectEventManager> logger;
    private readonly IEnumerable<IDomainEventHandler<Subject>> domainEventHandler;

    public SubjectEventManager(ILogger<SubjectEventManager> logger, IEnumerable<IDomainEventHandler<Subject>> domainEventHandler)
    {
        this.logger = logger;
        this.domainEventHandler = domainEventHandler;
    }

    public Task ProcessEvents(Subject aggregateRoot)
    {
        foreach (var eventHandler in domainEventHandler)
        {
            eventHandler.HandleEvent(aggregateRoot);
        }

        return Task.CompletedTask;
    }
}