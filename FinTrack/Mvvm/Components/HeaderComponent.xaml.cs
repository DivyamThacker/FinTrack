namespace FinTrack.Mvvm.Components;

using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.ComponentModel;

// HeaderComponent.xaml.cs
public partial class HeaderComponent : ContentView, INotifyPropertyChanged
{
    public static readonly BindableProperty UsernameLabelProperty =
        BindableProperty.Create(nameof(UsernameLabel), typeof(string), typeof(HeaderComponent), string.Empty, propertyChanged: OnUsernameLabelChanged);

    public string UsernameLabel
    {
        get => (string)GetValue(UsernameLabelProperty);
        set
        {
            SetValue(UsernameLabelProperty, value);
            OnPropertyChanged(nameof(UsernameLabel));
        }
    }

    public HeaderComponent()
    {
        InitializeComponent();
    }

    private static void OnUsernameLabelChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (HeaderComponent)bindable;
        control.OnPropertyChanged(nameof(UsernameLabel));
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
