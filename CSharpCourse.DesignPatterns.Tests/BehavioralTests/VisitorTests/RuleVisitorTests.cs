using CSharpCourse.DesignPatterns.Behavioral.Visitor;
using System.Text;

namespace CSharpCourse.DesignPatterns.Tests.BehavioralTests.VisitorTests;

public class RuleVisitorTests
{
    // We could also have used a builder to create the rules
    private readonly AndRule _passwordRule = new()
    {
        Rules =
        [
            new MinLengthRule { MinLength = 8 },
            new OrRule
            {
                Rules =
                [
                    new ContainsAtLeastOneCharacterRule
                    {
                        Characters = "0123456789"
                    },
                    new ContainsAtLeastOneCharacterRule
                    {
                        Characters = "!?%&*-_."
                    }
                ]
            }
        ]
    };

    private const string PasswordRequirementsMessage = """
                                                       All the following conditions must be true:
                                                         - The value must have at least 8 characters
                                                         - One of the following conditions must be true:
                                                           - The value must contain at least one of these characters: 0123456789
                                                           - The value must contain at least one of these characters: !?%&*-_.

                                                       """;

    [Fact]
    public void NoVisitor()
    {
        var passwordSb = new StringBuilder();
        BuildRequirementsMessage(passwordSb, _passwordRule);

        Assert.Equal(PasswordRequirementsMessage, passwordSb.ToString());

        var passwordValid = VerifyRule(_passwordRule, "admin");
        Assert.False(passwordValid);

        passwordValid = VerifyRule(_passwordRule, "admin123");
        Assert.True(passwordValid);
    }

    // We break the open-close principle
    private static void BuildRequirementsMessage(StringBuilder sb, IRule rule, int level = 0)
    {
        for (var i = 0; i < level; i++)
        {
            sb.Append("  ");
        }

        if (level > 0)
        {
            sb.Append("- ");
        }

        switch (rule)
        {
            case AndRule andRule:
                sb.AppendLine("All the following conditions must be true:");
                foreach (var subRule in andRule.Rules)
                {
                    BuildRequirementsMessage(sb, subRule, level + 1);
                }
                break;

            case OrRule orRule:
                sb.AppendLine("One of the following conditions must be true:");
                foreach (var subRule in orRule.Rules)
                {
                    BuildRequirementsMessage(sb, subRule, level + 1);
                }
                break;

            case MinLengthRule minLengthRule:
                sb
                    .Append("The value must have at least ")
                    .Append(minLengthRule.MinLength)
                    .AppendLine(" characters");
                break;

            case ContainsCharacterRule containsCharacterRule:
                sb
                    .Append("The value must contain the character ")
                    .Append(containsCharacterRule.Character)
                    .AppendLine();
                break;

            case ContainsAtLeastOneCharacterRule containsAtLeastOneCharacterRule:
                sb
                    .Append("The value must contain at least one of these characters: ")
                    .AppendLine(containsAtLeastOneCharacterRule.Characters);
                break;

            default:
                throw new NotImplementedException();
        }
    }

    // We break the open-close principle
    private static bool VerifyRule(IRule rule, string value)
    {
        return rule switch
        {
            AndRule andRule => andRule.Rules.All(r => VerifyRule(r, value)),
            OrRule orRule => orRule.Rules.Any(r => VerifyRule(r, value)),
            MinLengthRule minLengthRule => value.Length >= minLengthRule.MinLength,
            ContainsCharacterRule containsCharacterRule => value.Contains(containsCharacterRule.Character),
            ContainsAtLeastOneCharacterRule containsAtLeastOneCharacterRule => containsAtLeastOneCharacterRule.Characters.Any(value.Contains),
            _ => throw new NotImplementedException(),
        };
    }

    [Fact]
    public void ClassicVisitor()
    {
        var passwordRequirements = new RuleRequirementsBuilder();
        passwordRequirements.Visit(_passwordRule);

        Assert.Equal(PasswordRequirementsMessage, passwordRequirements.ToString());
    }

    [Fact]
    public void GenericVisitor()
    {
        var passwordRequirements = new RuleVerifier("admin");
        Assert.False(passwordRequirements.Visit(_passwordRule));

        passwordRequirements = new RuleVerifier("admin123");
        Assert.True(passwordRequirements.Visit(_passwordRule));
    }
}
