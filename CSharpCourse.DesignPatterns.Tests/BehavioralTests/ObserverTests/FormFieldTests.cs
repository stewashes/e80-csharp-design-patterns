using CSharpCourse.DesignPatterns.Behavioral.Observer;
using System.ComponentModel;

namespace CSharpCourse.DesignPatterns.Tests.BehavioralTests.ObserverTests;

public class FormFieldTests
{
    [Fact]
    public void TestFormField()
    {
        var formField = new FormField("initial value");
        string? currentValue = null;

        // We cannot use lambda expressions here, because we need to
        // remove the event handler when we are done, so we need a
        // persistent delegate to reference.
        // Lambda expressions are compiled to methods under the hood,
        // but we cannot reference them in our code.
        // In this case, we could also use a PropertyChangedEventHandler
        void UpdateValue(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FormField.Value))
            {
                currentValue = formField.Value;
            }
        }

        try
        {
            formField.PropertyChanged += UpdateValue;
            formField.Value = "new value";
            Assert.Equal("new value", currentValue);
        }
        finally
        {
            formField.PropertyChanged -= UpdateValue;
        }

        formField.Value = "another value";
        Assert.Equal("new value", currentValue);
    }
}
