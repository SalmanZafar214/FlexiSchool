﻿using Flexi.Domain.StudentAggregate;
using Flexi.Domain.StudentAggregate.ValueObjects;

namespace Flexi.Application.Students.Repository;

public interface IStudentRepository
{
    public Task<Student> GetById(StudentId studentId, CancellationToken cancellationToken);

    public Task Upsert(Student student, CancellationToken cancellationToken);
}