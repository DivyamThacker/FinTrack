//using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Maui.Controls;
using System.Xml.Linq;
using System.Diagnostics;
using System.Windows.Input;
using PropertyChanged;
using FinTrack.Mvvm.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.ComponentModel;
using FinTrack.Components.ChildComponents;

namespace FinTrack.Mvvm.Views;
//[AddINotifyPropertyChangedInterface]
public partial class OverviewPage : ContentPage
{
    //private Calculator calculatorComponent;
    //public string MyXamlProperty { get; set; } = "Initial Value";
    public bool IsDialogOpen { get; set; }
    public OverviewPage()
    {
        InitializeComponent();
        BindingContext = new OverviewViewModel();
        //calculatorComponent= new Calculator();
    }


    //private void OnCalculatorClicked()
    //{
    //    //calculatorComponent.ToggleDialog();
    //    //IsDialogOpen = !IsDialogOpen;
    //}

}