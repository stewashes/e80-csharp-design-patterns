using System.Text;

namespace CSharpCourse.DesignPatterns.Creational.Builder;

internal class MarkdownBuilder
{
    private readonly StringBuilder _builder = new();

    public void AddText(string text)
        => _builder.Append(text);

    public void AddHeader(int headerLevel, string text)
    {
        var prefix = new string(Enumerable.Repeat('#', headerLevel).ToArray());
        _builder.AppendLine($"{prefix} {text}");
    }

    public void AddBold(string text)
        => _builder.Append($"**{text}**");

    public void AddItalic(string text)
        => _builder.Append($"*{text}*");

    public void AddLink(string name, string url)
        => _builder.Append($"[{name}]({url})");

    public void NewLine()
        => _builder.AppendLine();

    public override string ToString()
        => _builder.ToString();
}
