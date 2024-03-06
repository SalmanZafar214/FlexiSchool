using Flexi.Domain.Core.Events;
using Flexi.Domain.StudentAggregate;
using Microsoft.Extensions.Logging;

namespace Flexi.Infrastructure.Students.EventManager;

internal class StudentEventManager : IDomainEventManager<Student>
{
    private readonly ILogger<StudentEventManager> logger;
    private readonly IEnumerable<IDomainEventHandler<Student>> domainEventHandler;

    public StudentEventManager(ILogger<StudentEventManager> logger, IEnumerable<IDomainEventHandler<Student>> domainEventHandler)
    {
        this.logger = logger;
        this.domainEventHandler = domainEventHandler;
    }

    public Task ProcessEvents(Student aggregateRoot)
    {
        foreach (var eventHandler in domainEventHandler)
        {
            eventHandler.HandleEvent(aggregateRoot);
        }

        return Task.CompletedTask;
    }
}