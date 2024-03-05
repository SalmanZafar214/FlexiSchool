using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Flexi.Domain.Core.Guard;

public static class Require
{
    public static void NotDefault<T>(T value,
        [CallerArgumentExpression("value")] string paramName = "")
    {
        if (value != null && value.Equals(default(T)))
        {
            throw new ArgumentException("Cannot be default", paramName);
        }
    }

    public static void NotNull<T>([NotNull] T? value,
        [CallerArgumentExpression("value")] string paramName = "")
    {
        if (value is null)
        {
            throw new ArgumentNullException(paramName);
        }
    }

    public static void NotNullOrEmpty([NotNull] string? value,
        [CallerArgumentExpression("value")] string paramName = "")
    {
        if (value is null)
        {
            throw new ArgumentNullException(paramName);
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Cannot be empty", paramName);
        }
    }

    public static void NotNullOrEmpty<T>([NotNull] List<T>? value,
        [CallerArgumentExpression("value")] string paramName = "")
    {
        if (value is null)
        {
            throw new ArgumentNullException(paramName);
        }

        var materializedCollection = value.ToList();

        if (!materializedCollection.Any())
        {
            throw new ArgumentException("Cannot be empty", paramName);
        }
    }
}