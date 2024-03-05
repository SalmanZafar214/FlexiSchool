using Microsoft.AspNetCore.Http;

namespace Flexi.Domain.Core.Exceptions;

public class NotFoundException : DomainException
{
    public string? EntityName { get; }
    public object? Id { get; }

    public NotFoundException(string entityName,
        object id,
        string? friendlyMessage = null) : base($"{entityName} with identifier '{id}' was not found", friendlyMessage, StatusCodes.Status404NotFound)
    {
        EntityName = entityName;
        Id = id;
    }

    public NotFoundException(string message,
        string? friendlyMessage = null) : base(message, friendlyMessage, StatusCodes.Status404NotFound)
    {
    }
}