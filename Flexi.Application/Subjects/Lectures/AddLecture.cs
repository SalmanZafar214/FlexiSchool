using Ardalis.ApiEndpoints;
using Flexi.Application.LectureTheaters.Repository;
using Flexi.Application.Subjects.Repository;
using Flexi.Domain.Core.Exceptions;
using Flexi.Domain.LectureTheaterAggregate.ValueObjects;
using Flexi.Domain.SubjectAggregate.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Flexi.Application.Subjects.Lectures;

public record CreateLectureRequest
{
    [FromRoute]
    public Guid SubjectId { get; init; }

    [FromBody]
    public CreateLectureRequestBody Body { get; init; } = default!;
}

public record CreateLectureRequestBody(Guid TheaterId, string Day, DateTime Time, int Duration);

[Route(Endpoints.Subjects.Lecture)]
public class AddLecture : EndpointBaseAsync
    .WithRequest<CreateLectureRequest>
    .WithActionResult
{
    private readonly ILogger<Create> logger;
    private readonly ISubjectRepository subjectRepository;
    private readonly ILectureTheaterRepository lectureTheaterRepository;

    public AddLecture(ILogger<Create> logger, ISubjectRepository subjectRepository, ILectureTheaterRepository lectureTheaterRepository)
    {
        this.logger = logger;
        this.subjectRepository = subjectRepository;
        this.lectureTheaterRepository = lectureTheaterRepository;
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Add a Lecture to the Subject",
        Description = "Add a lecture to the existing subject, returning the ID for the subject",
        OperationId = "Subject.Create",
        Tags = new[] { SwaggerTags.Lectures })]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public override async Task<ActionResult> HandleAsync(CreateLectureRequest request,
        CancellationToken cancellationToken = new())
    {
        var subjectId = SubjectId.Make(request.SubjectId);

        var existingSubject = await subjectRepository.GetById(subjectId, cancellationToken) ??
                              throw new NotFoundException($"Subject with id '{request.SubjectId} doe not  exists");

        var theaterId = LectureTheaterId.Make(request.Body.TheaterId);
        var dayOfWeek = Domain.SubjectAggregate.DayOfWeek.FromName(request.Body.Day);

        var existingLectureTheater = await lectureTheaterRepository.GetById(theaterId, cancellationToken) ??
                                     throw new NotFoundException($"LectureTheater with id '{request.Body.TheaterId} doe not  exists");

        var lectureCount = await subjectRepository.GetLectureCount(theaterId, dayOfWeek, cancellationToken);

        if (lectureCount > existingLectureTheater.Capacity)
        {
            throw new InvalidOperationException("Capacity exceeded");
        }

        var lecture = new Domain.SubjectAggregate.Lecture(LectureId.Make(),
                LectureTheaterId.Make(request.Body.TheaterId),
                Domain.SubjectAggregate.DayOfWeek.FromName(request.Body.Day),
                request.Body.Time,
                request.Body.Duration);

        existingSubject.AddLecture(lecture);

        await subjectRepository.Upsert(existingSubject, cancellationToken);

        return Ok();
    }
}