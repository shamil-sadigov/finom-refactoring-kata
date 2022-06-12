using System.Runtime.CompilerServices;

namespace ReportService.Application;

public static class ThrowingExtensions
{
    public static string ThrowIfNull(
        this string str,
        [CallerArgumentExpression("str")] string argName = "")
    {
        if (string.IsNullOrWhiteSpace(str)) 
            throw new ArgumentNullException(argName);
    }
}