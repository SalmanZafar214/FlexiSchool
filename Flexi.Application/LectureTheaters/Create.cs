using Ardalis.ApiEndpoints;
using Flexi.Application.LectureTheaters.Repository;
using Flexi.Domain.Core.ValueObjects;
using Flexi.Domain.LectureTheaterAggregate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Flexi.Application.LectureTheaters;

public record CreateLectureTheaterRequest
{
    [FromBody]
    [Required]
    public string Name { get; init; } = default!;

    [FromBody]
    [Required]
    public int Capacity { get; init; } = default!;
}

[Route(Endpoints.LectureTheater)]
public class Create : EndpointBaseAsync
    .WithRequest<CreateLectureTheaterRequest>
    .WithActionResult<CreateLectureTheaterResponse>
{
    private readonly ILogger<Create> logger;
    private readonly ILectureTheaterRepository lectureTheaterRepository;

    public Create(ILogger<Create> logger, ILectureTheaterRepository lectureTheaterRepository)
    {
        this.logger = logger;
        this.lectureTheaterRepository = lectureTheaterRepository;
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Creates a LectureTheater",
        Description = "Creates a LectureTheater, returning the ID for the lectureTheater",
        OperationId = "LectureTheater.Create",
        Tags = new[] { SwaggerTags.LectureTheater })]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CreateLectureTheaterResponse), StatusCodes.Status200OK)]
    public override async Task<ActionResult<CreateLectureTheaterResponse>> HandleAsync(CreateLectureTheaterRequest request,
        CancellationToken cancellationToken = new())
    {
        var name = request.Name.Trim();
        var performedBy = UserId.Make();

        var existingLectureTheater = await lectureTheaterRepository.GetByName(name, cancellationToken);

        if (existingLectureTheater is not null)
        {
            return BadRequest($"Lecture Theater '{name} already exists");
        }

        var lectureTheater = LectureTheater.Make(name, request.Capacity, performedBy, performedBy);

        await lectureTheaterRepository.Upsert(lectureTheater, cancellationToken);

        return Ok(new CreateLectureTheaterResponse(lectureTheater.Id.Value));
    }
}

public record CreateLectureTheaterResponse(Guid Id);