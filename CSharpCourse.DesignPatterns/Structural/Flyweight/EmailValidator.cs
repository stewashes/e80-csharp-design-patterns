using System.Text.RegularExpressions;

namespace CSharpCourse.DesignPatterns.Structural.Flyweight;

internal static partial class EmailValidator
{
    // The code for this regular expression is generated at compile time
    // through a source generator
    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase)]
    public static partial Regex EmailRegex();
}
