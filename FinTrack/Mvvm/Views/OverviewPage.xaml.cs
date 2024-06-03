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
using FinTrack.IViews;
using FinTrack.Services;
using FinTrack.Services.IServices;
using FinTrack.Helper;
using Syncfusion.Maui.Core.Carousel;

namespace FinTrack.Mvvm.Views;
//[AddINotifyPropertyChangedInterface]
public partial class OverviewPage : ContentPage, IStartPage
{
    //private Calculator calculatorComponent;
    //public string MyXamlProperty { get; set; } = "Initial Value";
    public OverviewViewModel MyViewModel { get; private set; }
    public bool IsDialogOpen { get; set; }
    public OverviewPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        var services = MauiProgram.CreateMauiApp().Services;
        var goalApiService = services.GetService<IGoalApiService>();
        var budgetApiService = services.GetService<IBudgetApiService>();
        var recordApiService = services.GetService<IRecordApiService>();
        var transactionApiService = services.GetService<ITransactionApiService>();
        var menuHandler = services.GetService<IMenuHandler>();
        MyViewModel = new OverviewViewModel(this.Navigation, budgetApiService, recordApiService, goalApiService, transactionApiService, menuHandler);
        BindingContext = MyViewModel;
    }

    private void OnMenuFlyoutItemClick(object sender, EventArgs e)
    {
        var item = (MenuFlyoutItem)sender;
        MenuBarHandler.Instance.HandleMenuFlyoutItemClick(item, Navigation);
    }

    protected override void OnDisappearing()
    {
        MyViewModel.Dispose();
        base.OnDisappearing();
    }

    //private void OnCalculatorClicked()
    //{
    //    //calculatorComponent.ToggleDialog();
    //    //IsDialogOpen = !IsDialogOpen;
    //}

}