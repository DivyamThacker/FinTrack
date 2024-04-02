using FinTrack.Mvvm.ViewModels;
using FinTrack_Models;

namespace FinTrack.Mvvm.Views;

public partial class GoalsPage : ContentPage
{
	public GoalsPage()
	{
		InitializeComponent();
        MyViewModel = new GoalsViewModel();
        BindingContext = MyViewModel;
    }
    public GoalsViewModel MyViewModel { get; private set; }

    private void GoalsListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item != null)
        {
            MyViewModel.SelectedGoal = (GoalDTO)e.Item;  // Update selected item in view model
            MyViewModel.SelectedItemCommand?.Execute(MyViewModel.SelectedGoal); // Execute the command if available
            // Optional: Perform additional logic with the selected item here
        }
    }
}