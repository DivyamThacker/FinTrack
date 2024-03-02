//using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Maui.Controls;
using System.Xml.Linq;
using FinTrack_DataAccess;
using System.Diagnostics;
using System.Windows.Input;
using PropertyChanged;
using FinTrack.Mvvm.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace FinTrack.Mvvm.Views;
//[AddINotifyPropertyChangedInterface]
public partial class OverviewPage : ContentPage
{
    public OverviewPage()
    {
        InitializeComponent();
        BindingContext = new OverviewViewModel();
        //WeakReferenceMessenger.Default.Register(this);
        //WeakReferenceMessenger.Default.Register<String>(this, (r, m) =>
        //{
        //    NavigationBtnClicked(m);
        //});
        WeakReferenceMessenger.Default.Register<MyMessage>(this, (r, m) =>
        {
            NavigationBtnClicked(m.Value);
        });
    }

    //private void NavigationBtnClicked(string value)
    //{
    //    Debug.WriteLine(value);
    //}


    private void NavigationBtnClicked(string value)
    {
        //ImageButton clickedButton = (ImageButton)sender;
        //string buttonId = ((ImageButton)sender).StyleId;
        switch (value)
        {
            case "Overview":
                Navigation.PushAsync(new OverviewPage());
                break;

            case "Transactions":
                Navigation.PushAsync(new TransactionsPage());
                break;

            case "Accounts":
                Navigation.PushAsync(new AccountsPage());
                break;

            case "Budget":
                Navigation.PushAsync(new BudgetPage());
                break;

            case "Goals":
                Navigation.PushAsync(new GoalsPage());
                break;

            case "Settings":
                Navigation.PushAsync(new SettingsPage());
                break;

            default: throw new Exception("No Such button found in this page");
        };
    }

}