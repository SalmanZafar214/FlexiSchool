using Ardalis.ApiEndpoints;
using Flexi.Application.Subjects.Repository;
using Flexi.Domain.StudentAggregate.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Flexi.Application.Subjects;

public class GetAllRequest
{
    [FromQuery]
    public int Limit { get; init; } = 10;

    [FromQuery]
    public int Page { get; set; } = 0;

    [FromQuery] public Guid? StudentId { get; set; }
}

[Route(Endpoints.Subjects.Subject)]
public class GetAll : EndpointBaseAsync
    .WithRequest<GetAllRequest>
    .WithActionResult<IEnumerable<GetAllResponse>>
{
    private readonly ILogger<GetAll> logger;
    private readonly ISubjectRepository subjectRepository;

    public GetAll(ILogger<GetAll> logger, ISubjectRepository subjectRepository)
    {
        this.logger = logger;
        this.subjectRepository = subjectRepository;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Gets All Subjects",
        OperationId = "Subject.GetAll",
        Tags = new[] { SwaggerTags.Subjects })]

    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<GetAllResponse>), StatusCodes.Status200OK)]
    public override async Task<ActionResult<IEnumerable<GetAllResponse>>> HandleAsync([FromRoute] GetAllRequest request,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var records = await subjectRepository.GetAll(
            request.StudentId.HasValue ? StudentId.Make(request.StudentId.Value) : null,
            request.Page,
            request.Limit,
            cancellationToken
        );

        return records.Select(r =>
            new GetAllResponse(r.Name,
                r.CreatedOn,
                r.Lectures.Select(l => new LectureResponse(
                    l.Id.Value,
                    l.DayOfWeek.Value,
                    l.TimeOfDay,
                    l.Duration,
                    l.TheaterId.Value)).ToList())).ToList();
    }
}

public record GetAllResponse(
    string Name,
    DateTime CreatedOn,
   List<LectureResponse>? Lectures);

public record LectureResponse(Guid Id, string Day, DateTime Time, int Duration, Guid TheaterId);