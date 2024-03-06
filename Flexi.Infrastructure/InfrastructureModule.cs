using Flexi.Application.LectureTheaters.Repository;
using Flexi.Application.Subjects.Repository;
using Flexi.Domain.Core.Aggregate;
using Flexi.Domain.Core.Events;
using Flexi.Domain.LectureTheaterAggregate.ValueObjects;
using Flexi.Domain.SubjectAggregate;
using Flexi.Domain.SubjectAggregate.ValueObjects;
using Flexi.Infrastructure.LectureTheaters;
using Flexi.Infrastructure.Mongo;
using Flexi.Infrastructure.Students;
using Flexi.Infrastructure.Subjects;
using Flexi.Infrastructure.Subjects.EventManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Flexi.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection RegisterInfrastructureDependencies(this IServiceCollection servicesCollection,
        IConfiguration configuration)
    {
        var section = configuration.GetSection(nameof(CollectionSettings));
        servicesCollection.Configure<CollectionSettings>(section);

        string connectionString = configuration.GetConnectionString("MongoDB");

        servicesCollection.AddSingleton<IMongoClient>(provider => new MongoClient(connectionString));

        servicesCollection.AddTransient<IDomainEventHandler<Subject>, SubjectCreatedEventHandler>();
        servicesCollection.AddTransient<IDomainEventHandler<Subject>, LectureAddedToSubjectEventHandler>();
        servicesCollection.AddTransient<IDomainEventManager<Subject>, SubjectEventManager>();

        RegisterSerializers();
        RegisterClassMaps();
        RegisterRepositories(servicesCollection);

        return servicesCollection;
    }

    private static void RegisterClassMaps()
    {
        BsonClassMap.RegisterClassMap<AggregateRoot<SubjectId>>(cm =>
        {
            cm.AutoMap();
            cm.SetIsRootClass(true);
        });

        BsonClassMap.RegisterClassMap<AggregateRoot<LectureTheaterId>>(cm =>
        {
            cm.AutoMap();
            cm.SetIsRootClass(true);
        });
    }

    private static void RegisterSerializers()
    {
        BsonSerializer.RegisterSerializer(new LectureTheaterIdSerializer());
        BsonSerializer.RegisterSerializer(new UserIdSerializer());
        BsonSerializer.RegisterSerializer(new DayOfWeekSerializer());
        BsonSerializer.RegisterSerializer(new LectureIdSerializer());
        BsonSerializer.RegisterSerializer(new StudentIdSerializer());
    }

    private static void RegisterRepositories(IServiceCollection services)
    {
        services.AddTransient<ILectureTheaterRepository, LectureTheaterRepository>();
        services.AddTransient<ISubjectRepository, SubjectRepository>();
    }
}