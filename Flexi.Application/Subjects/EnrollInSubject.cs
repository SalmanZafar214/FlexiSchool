using Ardalis.ApiEndpoints;
using Flexi.Application.Students.Repository;
using Flexi.Application.Subjects.Repository;
using Flexi.Domain.Core.Exceptions;
using Flexi.Domain.StudentAggregate.ValueObjects;
using Flexi.Domain.SubjectAggregate.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Flexi.Application.Subjects;

public record CreateEnrollInSubjectRequest
{
    [FromRoute]
    public Guid SubjectId { get; init; }

    [FromBody]
    public Guid StudentId { get; init; }
}

[Route(Endpoints.Subjects.Enroll)]
public class EnrollInSubject : EndpointBaseAsync
    .WithRequest<CreateEnrollInSubjectRequest>
    .WithActionResult
{
    private readonly ILogger<EnrollInSubject> logger;
    private readonly IStudentRepository studentRepository;
    private readonly ISubjectRepository subjectRepository;

    public EnrollInSubject(ILogger<EnrollInSubject> logger,
        IStudentRepository studentRepository,
        ISubjectRepository subjectRepository)
    {
        this.logger = logger;
        this.studentRepository = studentRepository;
        this.subjectRepository = subjectRepository;
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Enroll a student in subject",
        OperationId = "Subject.EnrollStudent",
        Tags = new[] { SwaggerTags.Subjects })]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public override async Task<ActionResult> HandleAsync([FromRoute] CreateEnrollInSubjectRequest request,
        CancellationToken cancellationToken = new())
    {
        var existingStudent = await studentRepository.GetById(StudentId.Make(request.StudentId), cancellationToken) ??
                              throw new NotFoundException($"Student with id {request.StudentId} does not exist");

        var existingSubject = await subjectRepository.GetById(SubjectId.Make(request.SubjectId), cancellationToken) ??
                              throw new NotFoundException($"Student with id {request.SubjectId} does not exist");

        existingSubject.EnrollInSubject(existingStudent);

        await subjectRepository.Upsert(existingSubject, cancellationToken);

        return Ok();
    }
}