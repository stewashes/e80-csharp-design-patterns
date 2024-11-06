using CSharpCourse.DesignPatterns.Structural.Flyweight;
using System.Text.RegularExpressions;

namespace CSharpCourse.DesignPatterns.Tests.StructuralTests.FlyweightTests;

public class EmailValidatorTests
{
    [Fact]
    public void Regex()
    {
        // Under the hood, the first time we create a Regex object
        // with a specific pattern, .NET compiles the pattern and
        // caches it for future use, so the second time we create
        // a Regex object with the same pattern, we don't need to
        // recompile it and it's faster.

        var regex = new Regex(@"\d+");
        var regex2 = new Regex(@"\d+");

        // We get different instances of the Regex object, even
        // though they refer to the same compiled pattern.
        Assert.NotSame(regex, regex2);
    }

    [Fact]
    public void CompiledRegex()
    {
        // The pattern is compiled at the program's compile time,
        // so we don't need to do it at runtime.

        var regex = EmailValidator.EmailRegex();
        var regex2 = EmailValidator.EmailRegex();

        // We get the same instance of the Regex object
        Assert.Same(regex, regex2);
    }
}
