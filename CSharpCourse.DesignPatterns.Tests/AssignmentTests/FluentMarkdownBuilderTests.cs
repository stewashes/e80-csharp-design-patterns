using CSharpCourse.DesignPatterns.Assignments;

namespace CSharpCourse.DesignPatterns.Tests.AssignmentTests;

public class FluentMarkdownBuilderTests
{
    [Fact]
    public void AddTable()
    {
        string[] headers = ["Tables", "Are", "Cool"];
        string[][] rows = [
            ["Hello", "World", "!!!"],
            ["One", "Two", "Three"]
        ];

        var output = new FluentMarkdownBuilder()
            .AddTable(headers, rows)
            .ToString();

        var expected = """
                       |Tables|Are|Cool|
                       |---|---|---|
                       |Hello|World|!!!|
                       |One|Two|Three|

                       """;

        Assert.Equal(expected, output);
    }

    [Fact]
    public void NestedMarkdown()
    {
        string[] headers = ["My", "Table"];

        var output = new FluentMarkdownBuilder()
            .AddTable(headers, table =>
            {
                table.AddRow(row =>
                    {
                        row.AddCell(cell =>
                        {
                            cell.AddLink(link =>
                            {
                                link.AddBold("Bold");
                            }, "https://example.com");
                        });
                        row.AddCell(cell =>
                        {
                            cell.AddItalic("Italic");
                        });
                    });
            })
            .ToString();

        var expected = """
                       |My|Table|
                       |---|---|
                       |[**Bold**](https://example.com)|*Italic*|

                       """;

        Assert.Equal(expected, output);
    }

    [Fact]
    public void CustomMarkdownTest()
    {
        string[] headers = ["My", "Test", "Table"];

        var output = new FluentMarkdownBuilder()
            .AddTable(headers, table =>
            {
                table.AddRow(row =>
                {
                    row.AddCell(cell =>
                    {
                        cell.AddLink(link =>
                        {
                            link.AddBold("test link bold");
                        }, "https://example.com");
                    });
                    row.AddCell(cell =>
                    {
                        cell.AddItalic("italic");
                    });
                    row.AddCell(cell =>
                    {
                        cell.AddLink(link =>
                        {
                            link.AddItalic("test link italic");
                        }, "https://example.com");
                    });
                });
                table.AddRow(row =>
                {
                    row.AddCell(cell =>
                    {
                        cell.AddText("text1");
                    });
                    row.AddCell(cell =>
                    {
                        cell.AddItalic("text2");
                    });
                    row.AddCell(cell =>
                    {
                        cell.AddText("text3");
                    });
                });
                table.AddRow(row =>
                {
                    row.AddCell(cell =>
                    {
                        cell.AddLink("text1", "www.text1.com");
                    });
                    row.AddCell(cell =>
                    {
                        cell.AddLink(link =>
                        {
                            link.AddText("text2");
                        },"www.text2.com");
                    });
                    row.AddCell(cell =>
                    {
                        cell.AddLink("text3", "www.text3.com");
                    });
                });
                table.AddRow(row =>
                {
                    row.AddCell(cell =>
                    {
                        cell.AddMonoSpace("mono");
                    });
                    row.AddCell(cell =>
                    {
                        cell.AddText("text");
                    });
                    row.AddEmptyCell();
                });
            })
            .ToString();

        var expected = """
                       |My|Test|Table|
                       |---|---|---|
                       |[**test link bold**](https://example.com)|*italic*|[*test link italic*](https://example.com)|
                       |text1|*text2*|text3|
                       |[text1](www.text1.com)|[text2](www.text2.com)|[text3](www.text3.com)|
                       |`mono`|text|---|
                       
                       """;

        Assert.Equal(expected, output);
    }
}
