using FinTrack.Helper;
using FinTrack.Mvvm.ViewModels;
//using FinTrack.Services;
//using FinTrack.Services.IServices;
using FinTrack_Common;
using FinTrack_Models;

namespace FinTrack.Mvvm.Views;

public partial class GoalsPage : ContentPage
{
    public GoalsViewModel MyViewModel { get; private set; }
	public GoalsPage()
	{
		InitializeComponent();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        var services = MauiProgram.CreateMauiApp().Services;
        //var goalApiService = services.GetService<IGoalApiService>();
        //var menuHandler = services.GetService<IMenuHandler>();
        //var navigation = services.GetService<INavigation>();
        //var preferences = services.GetService<IPreferences>();
        //MyViewModel = new GoalsViewModel(goalApiService, menuHandler, navigation, preferences);
        MyViewModel = services.GetService<GoalsViewModel>();
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

    private void GoalsListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item != null)
        {
            MyViewModel.SelectedGoal = (GoalDTO)e.Item;  // Update selected item in view model
            MyViewModel.SelectedItemCommand?.Execute(MyViewModel.SelectedGoal); // Execute the command if available
            // Optional: Perform additional logic with the selected item here
        }
    }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        MyViewModel.IsDatePickerVisible = picker.SelectedItem == SD.Period_Custom;
    }
}