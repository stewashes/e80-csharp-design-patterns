namespace CSharpCourse.DesignPatterns.Solid.Good;

internal interface IPdfGenerator
{
    void GeneratePdf(string content);
}

internal interface IWordDocumentGenerator
{
    void GenerateWordDocument(string content);
}

internal interface IExcelSheetGenerator
{
    void GenerateExcelSheet(string content);
}

internal interface IPowerPointGenerator
{
    void GeneratePowerPointPresentation(string content);
}

// This document processor can output to PDF, Word, Excel and PowerPoint
internal class FullDocumentProcessor : IPdfGenerator, IWordDocumentGenerator, IExcelSheetGenerator, IPowerPointGenerator
{
    public void GeneratePdf(string content)
        => Console.WriteLine("PDF Generated");

    public void GenerateWordDocument(string content)
        => Console.WriteLine("Word Document Generated");

    public void GenerateExcelSheet(string content)
        => Console.WriteLine("Excel Sheet Generated");

    public void GeneratePowerPointPresentation(string content)
        => Console.WriteLine("PowerPoint Presentation Generated");
}

// This document processor can only output to PDF and Word
internal class LimitedDocumentProcessor : IPdfGenerator, IWordDocumentGenerator
{
    public void GeneratePdf(string content)
        => Console.WriteLine("PDF Generated");

    public void GenerateWordDocument(string content)
        => Console.WriteLine("Word Document Generated");
}
