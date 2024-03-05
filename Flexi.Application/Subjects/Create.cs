using Ardalis.ApiEndpoints;
using Flexi.Application.LectureTheaters;
using Flexi.Application.Subjects.Repository;
using Flexi.Domain.Core.ValueObjects;
using Flexi.Domain.LectureTheaterAggregate.ValueObjects;
using Flexi.Domain.SubjectAggregate;
using Flexi.Domain.SubjectAggregate.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using DayOfWeek = Flexi.Domain.SubjectAggregate.DayOfWeek;

namespace Flexi.Application.Subjects;

public record CreateSubjectRequest
{
    [FromBody]
    public CreateSubjectRequestBody Body { get; init; } = default!;
}

public record Lecture(Guid TheaterId, string Day, DateTime Time, int Duration);

public record CreateSubjectRequestBody([Required] string Name, [Required] List<Lecture> Lectures);


[Route(Endpoints.Subjects.Subject)]
public class Create : EndpointBaseAsync
        .WithRequest<CreateSubjectRequest>
        .WithActionResult<CreateSubjectResponse>
{
    private readonly ILogger<Create> logger;
    private readonly ISubjectRepository subjectRepository;

    public Create(ILogger<Create> logger, ISubjectRepository subjectRepository)
    {
        this.logger = logger;
        this.subjectRepository = subjectRepository;
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Creates a Subject",
        Description = "Creates a Subject with the lectures, returning the ID for the subject",
        OperationId = "Subject.Create",
        Tags = new[] { SwaggerTags.Subjects })]

    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CreateSubjectResponse), StatusCodes.Status200OK)]
    public override async Task<ActionResult<CreateSubjectResponse>> HandleAsync(CreateSubjectRequest request,
        CancellationToken cancellationToken = new())
    {
        var name = request.Body.Name.Trim();
        var performedBy = UserId.Make();

        var existingSubject = await subjectRepository.GetByName(name, cancellationToken);

        if (existingSubject is not null)
        {
            return BadRequest($"Subject with same '{name} already exists");
        }

        var lectureTheater = Subject.Make(name,
            request.Body.Lectures.Select(l =>
                new Domain.SubjectAggregate.Lecture(LectureId.Make(),
                LectureTheaterId.Make(l.TheaterId),
                DayOfWeek.FromName(l.Day),
                l.Time,
                l.Duration)).ToList(),
            performedBy,
            performedBy);

        await subjectRepository.Upsert(lectureTheater, cancellationToken);

        return Ok(new CreateSubjectResponse(lectureTheater.Id.Value));
    }
}

public record CreateSubjectResponse(Guid Id);