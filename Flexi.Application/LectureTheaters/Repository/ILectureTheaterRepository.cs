using Flexi.Domain.LectureTheaterAggregate;

namespace Flexi.Application.LectureTheaters.Repository;

public interface ILectureTheaterRepository
{
    public Task<LectureTheater> GetByName(string name, CancellationToken cancellationToken);

    public Task Upsert(LectureTheater lectureTheater, CancellationToken cancellationToken);
}