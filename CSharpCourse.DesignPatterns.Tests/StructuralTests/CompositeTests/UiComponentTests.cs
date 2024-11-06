using CSharpCourse.DesignPatterns.Structural.Composite;
using CSharpCourse.DesignPatterns.Tests.Utils;

namespace CSharpCourse.DesignPatterns.Tests.StructuralTests.CompositeTests;

public class UiComponentTests
{
    [Fact]
    public void UiComponents()
    {
        var mainPanel = new Panel("MainPanel");
        mainPanel.Add(new Button("OkButton"));
        mainPanel.Add(new Button("CancelButton"));

        mainPanel.Render();

        var output = OutputUtils.CaptureConsoleOutput(mainPanel.Render);
        Assert.Contains("Rendering panel: MainPanel", output);
        Assert.Contains("Rendering button: OkButton", output);
        Assert.Contains("Rendering button: CancelButton", output);
    }
}
