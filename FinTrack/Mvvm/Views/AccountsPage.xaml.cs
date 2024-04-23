using FinTrack.Mvvm.ViewModels;

namespace FinTrack.Mvvm.Views;

public partial class AccountsPage : ContentPage
{
	public AccountsPage()
	{
		InitializeComponent();
        MyViewModel = new AccountsViewModel();
        BindingContext = MyViewModel;
    }

    public AccountsViewModel MyViewModel { get; private set; }

}
