namespace CSharpCourse.DesignPatterns.Tests.SolidTests;

public class InterfaceSegregationTests
{
    [Fact]
    public void Bad()
    {
        var full = new Solid.Bad.FullDocumentProcessor();
        full.GeneratePdf(string.Empty);
        full.GenerateWordDocument(string.Empty);
        full.GenerateExcelSheet(string.Empty);
        full.GeneratePowerPointPresentation(string.Empty);

        var limited = new Solid.Bad.LimitedDocumentProcessor();
        limited.GeneratePdf(string.Empty);
        limited.GenerateWordDocument(string.Empty);
        Assert.Throws<NotImplementedException>(() => limited.GenerateExcelSheet(string.Empty));
        Assert.Throws<NotImplementedException>(() => limited.GeneratePowerPointPresentation(string.Empty));
    }

    [Fact]
    public void Good()
    {
        var full = new Solid.Good.FullDocumentProcessor();
        full.GeneratePdf(string.Empty);
        full.GenerateWordDocument(string.Empty);
        full.GenerateExcelSheet(string.Empty);
        full.GeneratePowerPointPresentation(string.Empty);
        
        // We cannot call the other two methods since the
        // LimitedDocumentProcessor does not implement those interfaces
        var limited = new Solid.Good.LimitedDocumentProcessor();
        limited.GeneratePdf(string.Empty);
        limited.GenerateWordDocument(string.Empty);
        
        Assert.True(true);
    }
}
