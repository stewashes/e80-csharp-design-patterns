using System.Text;

namespace CSharpCourse.DesignPatterns.Assignments;

internal class FluentMarkdownBuilder
{
    private readonly StringBuilder _builder = new();

    public FluentMarkdownBuilder AddText(string text)
    {
        _builder.Append(text);
        return this;
    }

    public FluentMarkdownBuilder AddHeader(int headerLevel, string text)
    {
        var prefix = new string(Enumerable.Repeat('#', headerLevel).ToArray());
        _builder.AppendLine($"{prefix} {text}");
        return this;
    }

    public FluentMarkdownBuilder AddBold(string text)
    {
        _builder.Append($"**{text}**");
        return this;
    }

    public FluentMarkdownBuilder AddItalic(string text)
    {
        _builder.Append($"*{text}*");
        return this;
    }

    public FluentMarkdownBuilder AddLink(string name, string url)
    {
        _builder.Append($"[{name}]({url})");
        return this;
    }

    public FluentMarkdownBuilder NewLine()
    {
        _builder.AppendLine();
        return this;
    }

    public override string ToString()
        => _builder.ToString();
}
