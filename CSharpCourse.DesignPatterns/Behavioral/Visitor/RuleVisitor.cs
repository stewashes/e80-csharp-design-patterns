using System.Text;

namespace CSharpCourse.DesignPatterns.Behavioral.Visitor;

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
