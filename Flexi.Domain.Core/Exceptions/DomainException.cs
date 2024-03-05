using Microsoft.AspNetCore.Http;

namespace Flexi.Domain.Core.Exceptions;

public class DomainException : Exception
{
    public string? FriendlyMessage { get; }
    public int StatusCode { get; }

    public DomainException(string message,
        string? friendlyMessage,
        int? status,
        Exception? exception = null) : base(message, exception)
    {
        FriendlyMessage = friendlyMessage;
        StatusCode = status ?? StatusCodes.Status400BadRequest;
    }
}