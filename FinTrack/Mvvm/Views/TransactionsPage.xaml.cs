using FinTrack.Mvvm.ViewModels;
using FinTrack.Services;
using FinTrack.Services.IServices;
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
        //MyViewModel = new TransactionsViewModel(_transactionApiService);
        //BindingContext = MyViewModel;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        var services = MauiProgram.CreateMauiApp().Services;
        var transactionApiService = services.GetService<ITransactionApiService>();
        MyViewModel = new TransactionsViewModel(transactionApiService);
        BindingContext = MyViewModel;
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