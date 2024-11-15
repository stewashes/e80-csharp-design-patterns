using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CSharpCourse.DesignPatterns.Behavioral.Observer;

internal class FormField
{
    public FormField(string initialValue)
    {
        Value = initialValue;
    }

    public string Value { get; set; }
}
