using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CSharpCourse.DesignPatterns.Behavioral.Observer;

// The property observer is especially useful in WPF/WinForms
// applications, where we can bind the UI to the property, so that
// the UI updates automatically when the property changes (for both
// one-way and two-way bindings).

internal class FormField : INotifyPropertyChanged
{
    private string _value;
    public event PropertyChangedEventHandler? PropertyChanged;

    public FormField(string initialValue)
    {
        _value = initialValue;
    }

    public string Value
    {
        get => _value;
        set
        {
            _value = value;
            OnPropertyChanged(nameof(Value));
        }
    }

    // CallerMemberName will contain the name of the property that
    // called this method.
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
