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
        _builder
            .Append(prefix)
            .Append(' ')
            .Append(text);
        return this;
    }

    public FluentMarkdownBuilder AddBold(string text)
    {
        _builder
            .Append("**")
            .Append(text)
            .Append("**");
        return this;
    }

    public FluentMarkdownBuilder AddItalic(string text)
    {
        _builder
            .Append('*')
            .Append(text)
            .Append('*');
        return this;
    }

    public FluentMarkdownBuilder AddLink(string name, string url)
    {
        _builder
            .Append('[')
            .Append(name)
            .Append("](")
            .Append(url)
            .Append(')');
        return this;
    }

    public FluentMarkdownBuilder AddLink(Action<FluentMarkdownBuilder> builder,
        string link)
    {
        var innerBuilder = new FluentMarkdownBuilder();
        builder.Invoke(innerBuilder);
        AddLink(innerBuilder.ToString(), link);
        return this;
    }

    public FluentMarkdownBuilder AddTable(
        IEnumerable<string> headers,
        IEnumerable<IEnumerable<string>> rows)
    {
        /* 
         Sample output:

         |Tables|Are|Cool|
         |---|---|---|
         |Hello|World|!!!|
         |One|Two|Three|

         */

        // Headers
        _builder
            .Append('|')
            .AppendJoin('|', headers)
            .AppendLine("|");

        // Separator
        _builder
            .Append('|')
            .AppendJoin('|', headers.Select(_ => "---"))
            .AppendLine("|");

        foreach (var row in rows)
        {
            _builder
                .Append('|')
                .AppendJoin('|', row)
                .AppendLine("|");
        }

        return this;
    }

    public FluentMarkdownBuilder AddTable(IEnumerable<string> headers,
        Action<FluentMarkdownTableBuilder> builder)
    {
        AddTable(headers, []);
        var tableBuilder = new FluentMarkdownTableBuilder(_builder);
        builder.Invoke(tableBuilder);

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

internal class FluentMarkdownTableBuilder
{
    private readonly StringBuilder _builder;

    public FluentMarkdownTableBuilder(StringBuilder stringBuilder)
    {
        _builder = stringBuilder;
    }

    public FluentMarkdownTableBuilder AddRow(Action<FluentMarkdownRowBuilder> row)
    {
        _builder.Append('|');
        var rowBuilder = new FluentMarkdownRowBuilder(_builder);
        row.Invoke(rowBuilder);
        _builder.AppendLine();
        return this;
    }
}

internal class FluentMarkdownRowBuilder
{
    private readonly StringBuilder _builder;

    public FluentMarkdownRowBuilder(StringBuilder stringBuilder)
    {
        _builder = stringBuilder;
    }

    public FluentMarkdownRowBuilder AddCell(Action<FluentMarkdownBuilder> builder)
    {
        var innerBuilder = new FluentMarkdownBuilder();
        builder.Invoke(innerBuilder);
        _builder.Append(innerBuilder);
        _builder.Append('|');
        return this;
    }
}
