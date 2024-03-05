using Flexi.Domain.Core.Events;
using Flexi.Domain.SubjectAggregate.ValueObjects;

namespace Flexi.Domain.SubjectAggregate.Events;

public record SubjectCreated(SubjectId SubjectId, string Name) : IDomainEvent;