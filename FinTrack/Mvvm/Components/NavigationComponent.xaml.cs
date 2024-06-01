using CommunityToolkit.Mvvm.Messaging;
using FinTrack.Helper;
using FinTrack.IViews;
using FinTrack.Mvvm.ViewModels;
using FinTrack.Mvvm.Views;
using FinTrack.Services;
using System.Windows.Input;

namespace FinTrack.Mvvm.Components;

public partial class NavigationComponent : ContentView
{
    public List<NavigationItem> NavigationItems { get; set; } = new List<NavigationItem>();
    public NavigationComponent()
    {
        InitializeComponent();
        GetNavigationItems();
        BindingContext = this;
	}

    public void NavigationBtnClikced(string value) //object sender, EventArgs e
    {
        //BindingContext  = new OverviewViewModel(this.Navigation);
        switch (value)
        {
            case "Overview":
                Navigation.PushAsync(new OverviewPage());
                break;

            case "Transactions":
                Navigation.PushAsync(new TransactionsPage());
                break;

            case "Records":
                //Navigation.PushAsync(new RecordsPage());
                Navigation.PushAsync(ViewServices.ResolvePage<ISecondPage>());
                break;

            case "Accounts":
                Navigation.PushAsync(new AccountsPage());
                break;

            case "Budget":
                Navigation.PushAsync(new BudgetsPage());
                break;

            case "Goals":
                Navigation.PushAsync(new GoalsPage());
                break;

            case "Settings":
                Navigation.PushAsync(new SettingsPage());
                break;

            case "Calculator":
                //OnCalculatorClicked();
                break;

            default: throw new Exception("No Such button found in this page");
        };
    }

    private void GetNavigationItems()
    {
        NavigationItems.Add(new NavigationItem { Glyph = "\uf0e4", Text = "Overview" });
        NavigationItems.Add(new NavigationItem { Glyph = "\uf0ec", Text = "Transactions" });
        NavigationItems.Add(new NavigationItem { Glyph = "\ue814", Text = "Records" });
        NavigationItems.Add(new NavigationItem { Glyph = "\uf155", Text = "Budget" });
        NavigationItems.Add(new NavigationItem { Glyph = "\ue816", Text = "Goals" });
        //NavigationItems.Add(new NavigationItem { Glyph = "\uf0f2", Text = "Accounts" });
        //NavigationItems.Add(new NavigationItem { Glyph = "\ue80a", Text = "Settings" });
        //NavigationItems.Add(new NavigationItem { Glyph = "\uf1ec", Text = "Calculator" });
        foreach (var item in NavigationItems)
        {
            item.NavigationBtnCommand = new Command((text) =>
            {
                NavigationBtnClikced((string)text);
            });
        }
    }
}