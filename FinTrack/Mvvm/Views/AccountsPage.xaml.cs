using FinTrack.Helper;
using FinTrack.Mvvm.ViewModels;
using FinTrack.Services.IServices;

namespace FinTrack.Mvvm.Views;

public partial class AccountsPage : ContentPage
{
    public AccountsViewModel MyViewModel { get; private set; }
	public AccountsPage()
	{
		InitializeComponent();
        //MyViewModel = new AccountsViewModel();
        //BindingContext = MyViewModel;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        var services = MauiProgram.CreateMauiApp().Services;
        var recordApiService = services.GetService<IRecordApiService>();
        var menuHandler = services.GetService<IMenuHandler>();
        MyViewModel = new AccountsViewModel(recordApiService, menuHandler);
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


}
