using Flexi.Domain.Core.ValueObjects;

namespace Flexi.Infrastructure.Mongo;

internal sealed class UserIdSerializer : StringSerializer<UserId>
{
    protected override UserId FromString(string value)
    {
        var guid = new Guid(value);
        return UserId.Make(guid);
    }

    protected override string ToString(UserId value) =>
        value?.Value.ToString();
}