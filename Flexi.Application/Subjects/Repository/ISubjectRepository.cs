using Flexi.Domain.LectureTheaterAggregate.ValueObjects;
using Flexi.Domain.SubjectAggregate;
using Flexi.Domain.SubjectAggregate.ValueObjects;
using DayOfWeek = Flexi.Domain.SubjectAggregate.DayOfWeek;

namespace Flexi.Application.Subjects.Repository;

public interface ISubjectRepository
{
    public Task<Subject?> GetByName(string name, CancellationToken cancellationToken);

    public Task<List<Subject>> GetAll(int page, int limit, CancellationToken cancellationToken);

    public Task<Subject?> GetById(SubjectId subjectId, CancellationToken cancellationToken);

    public Task Upsert(Subject subject, CancellationToken cancellationToken);

    public Task<int> GetLectureCount(LectureTheaterId theaterId, DayOfWeek dayOfWeek,
        CancellationToken cancellationToken);
}