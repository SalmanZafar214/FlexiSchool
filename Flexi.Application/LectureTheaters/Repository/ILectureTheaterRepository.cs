using Flexi.Domain.LectureTheaterAggregate;
using Flexi.Domain.LectureTheaterAggregate.ValueObjects;

namespace Flexi.Application.LectureTheaters.Repository;

public interface ILectureTheaterRepository
{
    public Task<LectureTheater> GetByName(string name, CancellationToken cancellationToken);

    public Task<LectureTheater> GetById(LectureTheaterId lectureTheaterId, CancellationToken cancellationToken);

    public Task Upsert(LectureTheater lectureTheater, CancellationToken cancellationToken);
}