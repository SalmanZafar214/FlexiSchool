using Flexi.Domain.Core.Aggregate;

namespace Flexi.Domain.Core.Events;

public interface IDomainEventManager<in T> where T : IAggregateRoot
{
    Task ProcessEvents(T aggregateRoot);
}