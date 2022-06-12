using System.Runtime.CompilerServices;

namespace ReportService.Application;

public static class ThrowingExtensions
{
    public static void ThrowIfNull(
        this string str,
        [CallerArgumentExpression("str")] string argName = "")
    {
        if (string.IsNullOrWhiteSpace(str)) 
            throw new ArgumentNullException(argName);
    }
}