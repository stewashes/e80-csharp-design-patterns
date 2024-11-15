using System.Text;

namespace CSharpCourse.DesignPatterns.Assignments;

internal interface IRule
{
    
}

internal class AndRule : IRule
{
    public IEnumerable<IRule> Rules { get; set; } = [];
}

internal class OrRule : IRule
{
    public IEnumerable<IRule> Rules { get; set; } = [];
}

internal class MinLengthRule : IRule
{
    public int MinLength { get; set; }
}

internal class ContainsCharacterRule : IRule
{
    public char Character { get; set; }
}

internal class ContainsAtLeastOneCharacterRule : IRule
{
    public string Characters { get; set; } = string.Empty;
}

// IMPORTANT!
// This interface is already defined in the file and CANNOT be used to pass
// an input parameter to the visitor, since its generic type parameter
// is covariant and should only be used as a return type.
internal interface IRuleVisitor<out TResult>
{
    TResult Visit(AndRule rule);
    TResult Visit(OrRule rule);
    TResult Visit(MinLengthRule rule);
    TResult Visit(ContainsCharacterRule rule);
    TResult Visit(ContainsAtLeastOneCharacterRule rule);
}
