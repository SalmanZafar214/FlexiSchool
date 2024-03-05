using Flexi.Domain.Core.Events;
using Flexi.Domain.Core.Guard;
using Flexi.Domain.Core.ValueObjects;

namespace Flexi.Domain.Core.Aggregate;

public abstract class AggregateRoot<T> : IAggregateRoot where T : PrimitiveValueObject<Guid>
{
    private List<IDomainEvent>? events;

    public T Id { get; private set; }
    public UserId CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public DateTime ModifiedOn { get; private set; }
    public UserId ModifiedBy { get; private set; }

    public IEnumerable<IDomainEvent> Events => events is null
        ? new List<IDomainEvent>().AsReadOnly()
        : events.AsReadOnly();

    protected AggregateRoot(
        T id,
        UserId createdBy,
        DateTime createdOn,
        UserId modifiedBy,
        DateTime modifiedOn)
    {
        Require.NotNull(id);
        Require.NotNull(createdBy);
        Require.NotDefault(createdOn);
        Require.NotNull(modifiedBy);
        Require.NotDefault(createdOn);

        Require.NotDefault(modifiedOn);

        Id = id;
        CreatedBy = createdBy;
        CreatedOn = createdOn;
        ModifiedBy = modifiedBy;
        ModifiedOn = modifiedOn;

        events = new List<IDomainEvent>();
    }

    protected void AddEvent(IDomainEvent domainEvent)
    {
        Require.NotNull(domainEvent);

        events ??= new List<IDomainEvent>();
        events.Add(domainEvent);
    }
}