using Flexi.Application.Subjects.Repository;
using Flexi.Domain.Core.Events;
using Flexi.Domain.SubjectAggregate;
using Flexi.Domain.SubjectAggregate.ValueObjects;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Flexi.Infrastructure.Subjects;

public class SubjectRepository : ISubjectRepository
{
    private readonly IMongoCollection<Subject> subjectCollection;
    private readonly IDomainEventManager<Subject> subjectEventManager;

    public SubjectRepository(IOptions<CollectionSettings> settings, IMongoClient mongoClient,
        IDomainEventManager<Subject> subjectEventManager)
    {
        subjectCollection = mongoClient
            .GetDatabase(settings.Value.DatabaseName)
            .GetCollection<Subject>(settings.Value.SubjectCollectionName);
        this.subjectEventManager = subjectEventManager;
    }

    public async Task<Subject?> GetByName(string name, CancellationToken cancellationToken)
    {
        var filter = Builders<Subject>.Filter.Eq(p => p.Name, name);

        return await subjectCollection
            .Find(filter)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Subject>> GetAll(int page, int limit, CancellationToken cancellationToken)
    {
        var filter = Builders<Subject>.Filter.Empty;

        var results = await subjectCollection
            .Find(filter, new FindOptions()).Skip(page * limit)
            .Limit(limit)
            .ToListAsync(cancellationToken);

        return results;
    }

    public async Task<Subject?> GetById(SubjectId subjectId, CancellationToken cancellationToken)
    {
        var filter = Builders<Subject>.Filter.Eq(p => p.Id, subjectId);

        return await subjectCollection
            .Find(filter)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task Upsert(Subject subject, CancellationToken cancellationToken)
    {
        await subjectCollection.ReplaceOneAsync(
            Builders<Subject>.Filter.Eq(p => p.Id, subject.Id),
            options: new ReplaceOptions { IsUpsert = true },
            replacement: subject,
            cancellationToken: cancellationToken);

        await subjectEventManager.ProcessEvents(subject);
    }
}