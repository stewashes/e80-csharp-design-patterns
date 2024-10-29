using System.Text;

namespace CSharpCourse.DesignPatterns.Assignments;

internal class FluentMarkdownBuilder
{
    protected readonly StringBuilder Builder;

    public FluentMarkdownBuilder(StringBuilder? builder = null)
    {
        Builder = builder ?? new StringBuilder();
    }

    public FluentMarkdownBuilder AddText(string text)
    {
        Builder.Append(text);
        return this;
    }

    public FluentMarkdownBuilder AddHeader(int headerLevel, string text)
    {
        var prefix = new string(Enumerable.Repeat('#', headerLevel).ToArray());
        Builder.AppendLine($"{prefix} {text}");
        return this;
    }

    public FluentMarkdownBuilder AddBold(string text)
    {
        Builder.Append($"**{text}**");
        return this;
    }

    public FluentMarkdownBuilder AddItalic(string text)
    {
        Builder.Append($"*{text}*");
        return this;
    }

    public FluentMarkdownBuilder AddLink(string name, string url)
    {
        Builder.Append($"[{name}]({url})");
        return this;
    }

    public FluentMarkdownBuilder NewLine()
    {
        Builder.AppendLine();
        return this;
    }

    public FluentMarkdownBuilder AddMonoSpace(string text)
    {
        Builder.Append("`");
        Builder.Append(text);
        Builder.Append("`");
        return this;
    }

    public FluentMarkdownBuilder AddTable(string[] header, string[][] rows)
    {
        var nColumns = header.Length;
        
        for(int i = 0; i < nColumns; i++)
        {
            Builder.Append("|");
            Builder.Append(header[i]);
        }

        Builder.AppendLine("|");

        //var delimiter = string.Concat(Enumerable.Repeat("|---", nColumns).ToArray());
        //_builder.Append(delimiter);

        for (int i = 0; i < nColumns; i++)
        {
            Builder.Append("|---");
        }

        Builder.AppendLine("|");

        foreach (var i in rows)
        {
            foreach (var j in i)
            {
                Builder.Append("|");
                Builder.Append(j);
            }
            
            Builder.AppendLine("|");
        }
        
        return this;
    }


    public FluentMarkdownBuilder AddTable(string[] headers, Action<TableBuilder> tableBuilderAction)
    {
        tableBuilderAction(new TableBuilder(Builder, headers));
        return this;
    }

    public override string ToString()
        => Builder.ToString();
}

internal class TableBuilder : FluentMarkdownBuilder
{
    internal TableBuilder(StringBuilder builder, string[] header) : base(builder)
    {
        //for (int i = 0; i < header.Length; i++)
        //{
        //    Builder.Append("|");
        //    Builder.Append(header[i]);
        //}

        //Builder.AppendLine("|");

        //var delimiter = string.Concat(Enumerable.Repeat("|---", header.Length).ToArray());
        //Builder.Append(delimiter);
        
        //Builder.AppendLine("|");

        AddRow(row =>
        {
            for (int i = 0; i < header.Length; i++)
            {
                row.AddCell(cell => cell.AddText(header[i]));
            }
        });

        AddRow(row =>
        {
            for (int i = 0; i < header.Length; i++)
            {
                // row.AddCell(cell => cell.AddText("---"));
                row.AddEmptyCell();
            }
        });
    }

    public TableBuilder AddRow(Action<RowBuilder> rowBuilderAction)
    {
        rowBuilderAction(new RowBuilder(Builder));
        Builder.AppendLine("|");
        return this;
    }
}

internal class RowBuilder: FluentMarkdownBuilder
{
    internal RowBuilder(StringBuilder builder) : base(builder) { }

    public RowBuilder AddCell(Action<CellBuilder> cellBuilderAction)
    {
        Builder.Append("|");
        cellBuilderAction(new CellBuilder(Builder));

        return this;
    }

    public RowBuilder AddEmptyCell()
    {
        AddCell(cell => cell.AddText("---"));

        return this;
    }
}

internal class CellBuilder : FluentMarkdownBuilder
{
    internal CellBuilder(StringBuilder builder) : base(builder) { }

    public CellBuilder AddLink(Action<FluentMarkdownBuilder> linkBuilderAction, string link)
    {
        Builder.Append("[");

        linkBuilderAction(this);

        Builder.Append("](");
        Builder.Append(link);
        Builder.Append(")");

        return this;
    }
}
