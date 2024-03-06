using Flexi.Application.LectureTheaters.Repository;
using Flexi.Domain.LectureTheaterAggregate;
using Flexi.Domain.LectureTheaterAggregate.ValueObjects;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Flexi.Infrastructure.LectureTheaters;

public class LectureTheaterRepository : ILectureTheaterRepository
{
    private readonly IMongoCollection<LectureTheater> lectureTheaterCollection;

    public LectureTheaterRepository(IOptions<CollectionSettings> settings, IMongoClient mongoClient)
    {
        lectureTheaterCollection = mongoClient
            .GetDatabase(settings.Value.DatabaseName)
            .GetCollection<LectureTheater>(settings.Value.LectureTheaterCollectionName);
    }

    public async Task<LectureTheater> GetByName(string name, CancellationToken cancellationToken)
    {
        var filter = Builders<LectureTheater>.Filter.Eq(p => p.Name, name);

        return await lectureTheaterCollection
            .Find(filter)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<LectureTheater> GetById(LectureTheaterId lectureTheaterId, CancellationToken cancellationToken)
    {
        var filter = Builders<LectureTheater>.Filter.Eq(p => p.Id, lectureTheaterId);

        return await lectureTheaterCollection
            .Find(filter)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task Upsert(LectureTheater lectureTheater, CancellationToken cancellationToken)
    {
        await lectureTheaterCollection.ReplaceOneAsync(
            Builders<LectureTheater>.Filter.Eq(p => p.Id, lectureTheater.Id),
            options: new ReplaceOptions { IsUpsert = true },
            replacement: lectureTheater,
            cancellationToken: cancellationToken);
    }
}