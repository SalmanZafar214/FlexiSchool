using Flexi.Domain.Core.Guard;

namespace Flexi.Domain.Core.ValueObjects;

public class Email : PrimitiveValueObject<string>
{
    private Email(string value) : base(value)
    {
    }

    public static Email Make(string value)
    {
        Require.NotNullOrEmpty(value);

        value = value.ToLowerInvariant();

        try
        {
            var addr = new System.Net.Mail.MailAddress(value);
        }
        catch
        {
            throw new ArgumentException("Invalid Email", value);
        }
        return new Email(value);
    }

    public static Email? TryMake(string value)
    {
        try
        {
            return Make(value);
        }
        catch
        {
            return null;
        }
    }
}