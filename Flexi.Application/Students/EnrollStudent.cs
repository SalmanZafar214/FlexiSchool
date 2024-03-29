﻿using Ardalis.ApiEndpoints;
using Flexi.Application.Students.Repository;
using Flexi.Application.Subjects.Repository;
using Flexi.Domain.Core.Exceptions;
using Flexi.Domain.Core.ValueObjects;
using Flexi.Domain.StudentAggregate.ValueObjects;
using Flexi.Domain.SubjectAggregate.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Flexi.Application.Students;

public record CreateEnrollStudentRequest
{
    [FromRoute]
    public Guid StudentId { get; init; }

    [FromBody]
    public Guid SubjectId { get; init; } = default!;
}

[Route(Endpoints.Students.Enroll)]
public class EnrollStudent : EndpointBaseAsync
    .WithRequest<CreateEnrollStudentRequest>
    .WithActionResult
{
    private readonly ILogger<EnrollStudent> logger;
    private readonly IStudentRepository studentRepository;
    private readonly ISubjectRepository subjectRepository;

    public EnrollStudent(ILogger<EnrollStudent> logger,
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
        OperationId = "Student.Enroll",
        Tags = new[] { SwaggerTags.Students })]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public override async Task<ActionResult> HandleAsync([FromRoute] CreateEnrollStudentRequest request,
        CancellationToken cancellationToken = new())
    {
        var existingStudent = await studentRepository.GetById(StudentId.Make(request.StudentId), cancellationToken) ??
                              throw new NotFoundException($"Student with id {request.StudentId} does not exist");

        var existingSubject = await subjectRepository.GetById(SubjectId.Make(request.SubjectId), cancellationToken) ??
                              throw new NotFoundException($"Student with id {request.SubjectId} does not exist");
        existingStudent.EnrollInSubject(existingSubject);

        await studentRepository.Upsert(existingStudent, cancellationToken);

        return Ok();
    }
}