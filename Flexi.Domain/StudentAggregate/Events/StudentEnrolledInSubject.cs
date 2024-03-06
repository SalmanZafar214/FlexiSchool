using Flexi.Domain.Core.Events;
using Flexi.Domain.StudentAggregate.ValueObjects;
using Flexi.Domain.SubjectAggregate.ValueObjects;

namespace Flexi.Domain.StudentAggregate.Events;

public record StudentEnrolledInSubject(StudentId Id, 
    string StudentName, 
    SubjectId SubjectId, 
    string SubjectName) : IDomainEvent;