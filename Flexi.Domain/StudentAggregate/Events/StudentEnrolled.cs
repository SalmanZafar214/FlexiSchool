using Flexi.Domain.Core.Events;
using Flexi.Domain.StudentAggregate.ValueObjects;

namespace Flexi.Domain.StudentAggregate.Events;

public record StudentEnrolled(StudentId Id, string Name) : IDomainEvent;