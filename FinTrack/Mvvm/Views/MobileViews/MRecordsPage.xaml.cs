using FinTrack.Helper;
using FinTrack.IViews;
using FinTrack.Mvvm.ViewModels;
//using FinTrack.Services.IServices;

namespace FinTrack.Mvvm.Views.MobileViews;

public partial class MRecordsPage : ContentPage, ISecondPage
{
    public RecordsViewModel MyViewModel { get; private set; }
	public MRecordsPage()
	{
		InitializeComponent();
        //this.BindingContext = new RecordsViewModel(this.Navigation);
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        var services = MauiProgram.CreateMauiApp().Services;
        //var recordApiService = services.GetService<IRecordApiService>();
        //var menuHandler = services.GetService<IMenuHandler>();
        //MyViewModel = new RecordsViewModel(this.Navigation, recordApiService,menuHandler);
        MyViewModel = services.GetService<RecordsViewModel>();
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