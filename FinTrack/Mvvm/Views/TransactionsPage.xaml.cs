using FinTrack.Helper;
using FinTrack.Mvvm.ViewModels;
//using FinTrack.Services;
//using FinTrack.Services.IServices;
using FinTrack_Models;
using System.Diagnostics;
using System.Windows.Input;

namespace FinTrack.Mvvm.Views;

public partial class TransactionsPage : ContentPage
{
    public TransactionsViewModel MyViewModel { get; private set; }
    public TransactionsPage()
	{
		InitializeComponent();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        var services = MauiProgram.CreateMauiApp().Services;
        //var transactionApiService = services.GetService<ITransactionApiService>();
        //var menuHandler = services.GetService<IMenuHandler>();
        //MyViewModel = new TransactionsViewModel(transactionApiService, menuHandler);
        MyViewModel = services.GetService<TransactionsViewModel>();
        MyViewModel.Navigation = this.Navigation;
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
    private void TransactionsListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item != null)
        {
            MyViewModel.SelectedTransaction = (TransactionDTO)e.Item;  // Update selected item in view model
            MyViewModel.SelectedItemCommand?.Execute(MyViewModel.SelectedTransaction); // Execute the command if available
            // Optional: Perform additional logic with the selected item here
        }
    }
}