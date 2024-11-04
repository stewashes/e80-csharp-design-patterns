namespace CSharpCourse.DesignPatterns.Tests.StructuralTests.FlyweightTests;

public class StringInterningTests
{
    // .NET's string interning mechanism.
    // The CLR maintains an internal pool to store unique string literals,
    // ensuring that identical strings share the same memory.

    [Fact]
    public void TestStringInterning()
    {
        string str1 = "hello";
        string str2 = "hello";
        string str3 = new([ 'h', 'e', 'l', 'l', 'o' ]);

        // str1 and str2 reference the same interned string
        Assert.True(ReferenceEquals(str1, str2));

        // str3 is a new instance (there are two copies of the
        // string "hello" in memory)
        Assert.False(ReferenceEquals(str1, str3));

        // Intern str3 to make it reference the same string as str1
        // The CLR checks if the string is already in the pool and
        // returns the reference to the existing string if it is.
        string internedStr3 = string.Intern(str3);
        Assert.True(ReferenceEquals(str1, internedStr3));
    }
}
