//using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Maui.Controls;
using System.Xml.Linq;
using FinTrack_DataAccess;
using System.Diagnostics;
using System.Windows.Input;
using PropertyChanged;
using FinTrack.Mvvm.ViewModels;

namespace FinTrack.Mvvm.Views;
//[AddINotifyPropertyChangedInterface]
public partial class OverviewPage : ContentPage
{


    public OverviewPage()
    {
        InitializeComponent();
        BindingContext = new OverviewViewModel();
    }

    private void NavigationBtnClicked(object obj)
    {
        string button = (string)obj;
        //ImageButton clickedButton = (ImageButton)sender;
        //string buttonId = ((ImageButton)sender).StyleId;
        switch (button)
        {
            case "Overview":
                Navigation.PushAsync(new OverviewPage());
                break;

            case "Transactions":
                Navigation.PushAsync(new TransactionsPage());
                break;

            //case "OverviewBtn":
            //    Navigation.PushAsync(new OverviewPage());
            //    break; 

            //case "OverviewBtn":
            //    Navigation.PushAsync(new OverviewPage());
            //    break; 

            //case "OverviewBtn":
            //    Navigation.PushAsync(new OverviewPage());
            //    break; 

            //case "OverviewBtn":
            //    Navigation.PushAsync(new OverviewPage());
            //    break;

            default: throw new Exception("No Such button found in this page");
        };
    }

}