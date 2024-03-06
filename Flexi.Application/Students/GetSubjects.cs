using Ardalis.ApiEndpoints;
using Flexi.Application.Students.Repository;
using Flexi.Domain.Core.Exceptions;
using Flexi.Domain.StudentAggregate.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Flexi.Application.Students;

public record GetStudentRequest([FromRoute] Guid StudentId);

[Route(Endpoints.Students.StudentById)]
public class GetSubjects : EndpointBaseAsync
    .WithRequest<GetStudentRequest>
    .WithActionResult<List<GetStudentResponse>>
{
    private readonly ILogger<GetSubjects> logger;
    private readonly IStudentRepository studentRepository;

    public GetSubjects(ILogger<GetSubjects> logger,
        IStudentRepository studentRepository)
    {
        this.logger = logger;
        this.studentRepository = studentRepository;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Gets subject of the student",
        OperationId = "Student.GetSubjects",
        Tags = new[] { SwaggerTags.Students })]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public override async Task<ActionResult<List<GetStudentResponse>>> HandleAsync([FromRoute]GetStudentRequest request,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var existingStudent = await studentRepository.GetById(StudentId.Make(request.StudentId), cancellationToken) ??
                              throw new NotFoundException($"Student with id {request.StudentId} does not exist");

        var subjects = existingStudent.Subjects.Select(s => new GetStudentResponse(s.Name)).ToList();

        return subjects;
    }
}

public record GetStudentResponse(string Name);