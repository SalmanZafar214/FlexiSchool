using Flexi.Application.Students.Repository;
using Flexi.Domain.Core.Events;
using Flexi.Domain.StudentAggregate;
using Flexi.Domain.StudentAggregate.ValueObjects;
using Flexi.Domain.SubjectAggregate;
using Flexi.Domain.SubjectAggregate.ValueObjects;
using Flexi.Infrastructure.Subjects.EventManager;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Flexi.Infrastructure.Students;

internal class StudentRepository : IStudentRepository
{
    private readonly IMongoCollection<Student> studentCollection;
    private readonly IDomainEventManager<Student> studentEventManager;

    public StudentRepository(IOptions<CollectionSettings> settings, IMongoClient mongoClient,
        IDomainEventManager<Student> studentEventManager)
    {
        studentCollection = mongoClient
            .GetDatabase(settings.Value.DatabaseName)
            .GetCollection<Student>(settings.Value.StudentCollectionName);
        this.studentEventManager = studentEventManager;
    }

    public async Task<Student> GetById(StudentId studentId, CancellationToken cancellationToken)
    {
        var filter = Builders<Student>.Filter.Eq(p => p.Id, studentId);

        return await studentCollection
            .Find(filter)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task Upsert(Student student, CancellationToken cancellationToken)
    {
        await studentCollection.ReplaceOneAsync(
            Builders<Student>.Filter.Eq(p => p.Id, student.Id),
            options: new ReplaceOptions { IsUpsert = true },
            replacement: student,
        cancellationToken: cancellationToken);

        await studentEventManager.ProcessEvents(student);
    }
}