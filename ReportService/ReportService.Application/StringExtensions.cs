using System.Runtime.CompilerServices;

namespace ReportService.Application;

public static class StringExtensions
{
    public static string ThrowIfNull(
        this string str,
        [CallerArgumentExpression("str")] string argName = "")
    {
        if (string.IsNullOrWhiteSpace(str)) 
            throw new ArgumentNullException(argName);
        
        return str;
    }
}
