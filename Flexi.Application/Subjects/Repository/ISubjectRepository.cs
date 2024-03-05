using Flexi.Domain.SubjectAggregate;
using Flexi.Domain.SubjectAggregate.ValueObjects;

namespace Flexi.Application.Subjects.Repository;

public interface ISubjectRepository
{
    public Task<Subject?> GetByName(string name, CancellationToken cancellationToken);

    public Task<List<Subject>> GetAll(int page, int limit, CancellationToken cancellationToken);

    public Task<Subject?> GetById(SubjectId subjectId, CancellationToken cancellationToken);

    public Task Upsert(Subject subject, CancellationToken cancellationToken);
}