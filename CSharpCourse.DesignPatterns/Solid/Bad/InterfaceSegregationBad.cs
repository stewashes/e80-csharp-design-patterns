namespace CSharpCourse.DesignPatterns.Solid.Bad;

internal interface IDocumentProcessor
{
    void GeneratePdf(string content);
    void GenerateWordDocument(string content);
    void GenerateExcelSheet(string content);
    void GeneratePowerPointPresentation(string content);
}

// This document processor can output to PDF, Word, Excel and PowerPoint
internal class FullDocumentProcessor : IDocumentProcessor
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
internal class LimitedDocumentProcessor : IDocumentProcessor
{
    public void GeneratePdf(string content)
        => Console.WriteLine("PDF Generated");

    public void GenerateWordDocument(string content)
        => Console.WriteLine("Word Document Generated");

    public void GenerateExcelSheet(string content)
        => throw new NotImplementedException();

    public void GeneratePowerPointPresentation(string content)
        => throw new NotImplementedException();
}
