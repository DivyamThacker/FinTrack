using FinTrack.Mvvm.ViewModels;
using FinTrack.Services;
using FinTrack.Services.IServices;
using FinTrack_Common;
using FinTrack_Models;

namespace FinTrack.Mvvm.Views;

public partial class GoalsPage : ContentPage
{
    public GoalsViewModel MyViewModel { get; private set; }
	public GoalsPage()
	{
		InitializeComponent();
        //MyViewModel = new GoalsViewModel(_goalApiService);
        //BindingContext = MyViewModel;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        var services = MauiProgram.CreateMauiApp().Services;
        var goalApiService = services.GetService<IGoalApiService>();
        MyViewModel = new GoalsViewModel(goalApiService);
        BindingContext = MyViewModel;
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