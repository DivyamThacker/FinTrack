using FinTrack.Mvvm.ViewModels;
using FinTrack_Models;
using System.Diagnostics;
using System.Windows.Input;

namespace FinTrack.Mvvm.Views;

public partial class TransactionsPage : ContentPage
{
    public TransactionsPage()
	{
		InitializeComponent();
        MyViewModel = new TransactionsViewModel();
        BindingContext = MyViewModel;
    }
    public TransactionsViewModel MyViewModel { get; private set; }

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