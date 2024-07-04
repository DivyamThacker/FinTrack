
using FinTrack.Mvvm.ViewModels;
using Microsoft.AspNetCore.Components.WebView.Maui;
using System.ComponentModel;

namespace FinTrack;
public partial class BlazorHostPage : ContentPage
{
    private readonly BlazorHostViewModel ViewModel;

    public BlazorHostPage(string pageName)
    {
        InitializeComponent();

        ViewModel = new BlazorHostViewModel();
        BindingContext = ViewModel;

        // Set the initial Blazor component based on the provided page name
        ViewModel.PageName = pageName;

        // Set the initial Blazor component
        UpdateBlazorComponent();

        // Handle property changes to update the Blazor component
        ViewModel.PropertyChanged += OnViewModelPropertyChanged;
    }

    private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ViewModel.PageName))
        {
            UpdateBlazorComponent();
        }
    }

    private void UpdateBlazorComponent()
    {
        //BlazorWebViewControl.RootComponents.Clear();

        BlazorWebViewControl.StartPath = $"{ViewModel.PageName}";

        //BlazorWebViewControl.RootComponents.Add(new RootComponent()
        //{
        //    Selector = "#app",
        //    ComponentType = ViewModel.BlazorPage
        //});
    }
}
