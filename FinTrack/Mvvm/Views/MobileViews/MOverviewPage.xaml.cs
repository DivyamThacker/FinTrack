using FinTrack.Helper;
using FinTrack.IViews;
using FinTrack.Mvvm.ViewModels;
//using FinTrack.Services;
//using FinTrack.Services.IServices;

namespace FinTrack.Mvvm.Views.MobileViews;

public partial class MOverviewPage : ContentPage, IStartPage
{
    public OverviewViewModel MyViewModel { get; private set; }
    public MOverviewPage()
	{
		InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        var services = MauiProgram.CreateMauiApp().Services;
        //var goalApiService = services.GetService<IGoalApiService>();
        //var budgetApiService = services.GetService<IBudgetApiService>();
        //var recordApiService = services.GetService<IRecordApiService>();
        //var transactionApiService = services.GetService<ITransactionApiService>();
        //var menuHandler = services.GetService<IMenuHandler>();
        //var preferences = services.GetService<IPreferences>();
        //MyViewModel = new OverviewViewModel(this.Navigation, budgetApiService, recordApiService, goalApiService, transactionApiService, menuHandler, preferences);
        MyViewModel = services.GetService<OverviewViewModel>();
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
}