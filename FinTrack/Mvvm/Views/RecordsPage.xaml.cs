//using AndroidX.Lifecycle;
using FinTrack.Helper;
using FinTrack.IViews;
using FinTrack.Mvvm.ViewModels;
using FinTrack.Services.IServices;
using FinTrack_Models;

namespace FinTrack.Mvvm.Views;

public partial class RecordsPage : ContentPage, ISecondPage
{
    public RecordsViewModel MyViewModel { get; private set; }
    public RecordsPage()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();

        var services = MauiProgram.CreateMauiApp().Services;
        var recordApiService = services.GetService<IRecordApiService>();
        var menuHandler = services.GetService<IMenuHandler>();
        MyViewModel = new RecordsViewModel(this.Navigation,recordApiService, menuHandler);
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

    private void RecordsListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item != null)
        {
            MyViewModel.SelectedRecord = (RecordDTO)e.Item;  // Update selected item in view model
            MyViewModel.SelectedItemCommand?.Execute(MyViewModel.SelectedRecord); // Execute the command if available
            // Optional: Perform additional logic with the selected item here
        }
    }
}