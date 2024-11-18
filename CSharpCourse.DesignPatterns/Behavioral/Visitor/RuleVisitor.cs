using System.Text;

namespace CSharpCourse.DesignPatterns.Behavioral.Visitor;

internal interface IRule
{
    // This allows double dispatch
    void Accept(IRuleVisitor visitor);
    TResult Accept<TResult>(IRuleVisitor<TResult> visitor);
}

internal class AndRule : IRule
{
    public IEnumerable<IRule> Rules { get; set; } = [];

    // Each rule knows how to accept a visitor
    public void Accept(IRuleVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IRuleVisitor<TResult> visitor) => visitor.Visit(this);
}

internal class OrRule : IRule
{
    public IEnumerable<IRule> Rules { get; set; } = [];
    public void Accept(IRuleVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IRuleVisitor<TResult> visitor) => visitor.Visit(this);
}

internal class MinLengthRule : IRule
{
    public int MinLength { get; set; }
    public void Accept(IRuleVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IRuleVisitor<TResult> visitor) => visitor.Visit(this);
}

internal class ContainsCharacterRule : IRule
{
    public char Character { get; set; }
    public void Accept(IRuleVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IRuleVisitor<TResult> visitor) => visitor.Visit(this);
}

internal class ContainsAtLeastOneCharacterRule : IRule
{
    public string Characters { get; set; } = string.Empty;
    public void Accept(IRuleVisitor visitor) => visitor.Visit(this);
    public TResult Accept<TResult>(IRuleVisitor<TResult> visitor) => visitor.Visit(this);
}

// Unlike with the composite pattern, when we want to implement a new
// operation on the rule tree, we can do so by creating a new visitor
// class, instead of modifying the existing rule classes.

// Classic visitor
internal interface IRuleVisitor
{
    void Visit(AndRule rule);
    void Visit(OrRule rule);
    void Visit(MinLengthRule rule);
    void Visit(ContainsCharacterRule rule);
    void Visit(ContainsAtLeastOneCharacterRule rule);
}

// Generic visitor
internal interface IRuleVisitor<out TResult>
{
    TResult Visit(AndRule rule);
    TResult Visit(OrRule rule);
    TResult Visit(MinLengthRule rule);
    TResult Visit(ContainsCharacterRule rule);
    TResult Visit(ContainsAtLeastOneCharacterRule rule);
}

internal class RuleRequirementsBuilder : IRuleVisitor
{
    // This visitor is stateful, since we need to keep track of the
    // current level of the rule tree, and we also need a centralized
    // reference to the StringBuilder.

    private readonly StringBuilder _sb = new();
    public int Level { get; set; } = 0;

    public void Visit(AndRule rule)
    {
        AppendSpace(Level);

        var level = Level;

        _sb.AppendLine("All the following conditions must be true:");
        foreach (var subRule in rule.Rules)
        {
            Level = level + 1;
            subRule.Accept(this);
        }
    }

    public void Visit(OrRule rule)
    {
        AppendSpace(Level);

        var level = Level;

        _sb.AppendLine("One of the following conditions must be true:");
        foreach (var subRule in rule.Rules)
        {
            Level = level + 1;
            subRule.Accept(this);
        }
    }

    public void Visit(MinLengthRule rule)
    {
        AppendSpace(Level);
        
        _sb
            .Append("The value must have at least ")
            .Append(rule.MinLength)
            .AppendLine(" characters");
    }

    public void Visit(ContainsCharacterRule rule)
    {
        AppendSpace(Level);

        _sb
            .Append("The value must contain the character ")
            .Append(rule.Character)
            .AppendLine();
    }

    public void Visit(ContainsAtLeastOneCharacterRule rule)
    {
        AppendSpace(Level);
        
        _sb
            .Append("The value must contain at least one of these characters: ")
            .AppendLine(rule.Characters);
    }

    private void AppendSpace(int level)
    {
        for (var i = 0; i < level; i++)
        {
            _sb.Append("  ");
        }

        if (level > 0)
        {
            _sb.Append("- ");
        }
    }

    public override string ToString() => _sb.ToString();
}

internal class RuleVerifier : IRuleVisitor<bool>
{
    public string Value { get; }

    public RuleVerifier(string value)
    {
        Value = value;
    }

    public bool Visit(AndRule rule) => rule.Rules.All(r => r.Accept(this));
    public bool Visit(OrRule rule) => rule.Rules.Any(r => r.Accept(this));
    public bool Visit(MinLengthRule rule) => Value.Length >= rule.MinLength;
    public bool Visit(ContainsCharacterRule rule) => Value.Contains(rule.Character);
    public bool Visit(ContainsAtLeastOneCharacterRule rule) => rule.Characters.Any(c => Value.Contains(c));
}
