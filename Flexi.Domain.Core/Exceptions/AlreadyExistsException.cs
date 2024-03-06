using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flexi.Domain.Core.Exceptions;

public class AlreadyExistsException : DomainException
{
    public AlreadyExistsException(string id,
        string entityName,
        string? friendlyMessage = null) : base($"{id} on {entityName} already exists", friendlyMessage,
        StatusCodes.Status409Conflict)
    {
    }
}