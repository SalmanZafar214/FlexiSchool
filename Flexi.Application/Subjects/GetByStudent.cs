using Ardalis.ApiEndpoints;
using Flexi.Application.Students;
using Flexi.Application.Subjects.Repository;
using Flexi.Domain.Core.Exceptions;
using Flexi.Domain.StudentAggregate.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Flexi.Application.Subjects;

public record GetByStudentRequest([FromQuery] Guid StudentId);

[Route(Endpoints.Subjects.SubjectById)]
public class GetByStudent : EndpointBaseAsync
    .WithRequest<GetByStudentRequest>
    .WithActionResult<List<GetByStudentResponse>>
{
    private readonly ILogger<GetSubjects> logger;
    private readonly ISubjectRepository subjectRepository;

    public GetByStudent(ILogger<GetSubjects> logger,
        ISubjectRepository subjectRepository)
    {
        this.logger = logger;
        this.subjectRepository = subjectRepository;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Gets subjects associated to the student",
        OperationId = "Subject.GetByStudent",
        Tags = new[] { SwaggerTags.Subjects })]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public override async Task<ActionResult<List<GetByStudentResponse>>> HandleAsync(GetByStudentRequest request,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var subjects = await subjectRepository.GetByStudent(StudentId.Make(request.StudentId), cancellationToken) ??
                              throw new NotFoundException($"Student with id {request.StudentId} does not exist");

        return subjects.Select(s => new GetByStudentResponse(s.Name)).ToList(); ;
    }
}

public record GetByStudentResponse(string Name);