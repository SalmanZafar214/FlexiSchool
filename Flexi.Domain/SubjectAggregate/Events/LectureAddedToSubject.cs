using Flexi.Domain.Core.Events;
using Flexi.Domain.SubjectAggregate.ValueObjects;

namespace Flexi.Domain.SubjectAggregate.Events;

public record LectureAddedToSubject(LectureId LectureId, string SubjectName, SubjectId SubjectId) : IDomainEvent;