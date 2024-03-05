using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flexi.Domain.Core.Events;

public interface IDomainEventHandler<T>
{
    Task HandleEvent(T entity);
}