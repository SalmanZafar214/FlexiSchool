using Ardalis.ApiEndpoints;
using Flexi.Application.Students.Repository;
using Flexi.Domain.Core.Exceptions;
using Flexi.Domain.Core.ValueObjects;
using Flexi.Domain.StudentAggregate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Flexi.Application.Students;

public record CreateStudentRequest(string Name, string Email);

[Route(Endpoints.Students.Student)]
public class Create : EndpointBaseAsync
    .WithRequest<CreateStudentRequest>
    .WithActionResult
{
    private readonly ILogger<EnrollStudent> logger;
    private readonly IStudentRepository studentRepository;

    public Create(ILogger<EnrollStudent> logger,
        IStudentRepository studentRepository)
    {
        this.logger = logger;
        this.studentRepository = studentRepository;
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Creates a student",
        OperationId = "Student.Create",
        Tags = new[] { SwaggerTags.Students })]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public override async Task<ActionResult> HandleAsync(CreateStudentRequest request,
        CancellationToken cancellationToken = new())
    {
        var performedBy = UserId.Make();
        var existingStudent = await studentRepository.GetByEmail(Email.Make(request.Email), cancellationToken) ??
                              throw new NotFoundException($"Student with email {request.Email} already exist");
        var student = Student.Make(request.Name, request.Email, performedBy, performedBy);

        await studentRepository.Upsert(student, cancellationToken);
        return Ok(new CreateStudentResponse(student.Id.Value));
    }
}

public record CreateStudentResponse(Guid Id);