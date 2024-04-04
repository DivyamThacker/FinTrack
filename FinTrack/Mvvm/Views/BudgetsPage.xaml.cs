using FinTrack.Mvvm.ViewModels;
using FinTrack_Common;
using FinTrack_Models;

namespace FinTrack.Mvvm.Views;

public partial class BudgetsPage : ContentPage
{
	public BudgetsPage()
	{
		InitializeComponent();
        MyViewModel = new BudgetsViewModel();
        BindingContext = MyViewModel;
    }
    public BudgetsViewModel MyViewModel { get; private set; }

    private void BudgetsListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item != null)
        {
            MyViewModel.SelectedBudget = (BudgetDTO)e.Item;  // Update selected item in view model
            MyViewModel.SelectedItemCommand?.Execute(MyViewModel.SelectedBudget); // Execute the command if available
            // Optional: Perform additional logic with the selected item here
        }
    }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        MyViewModel.IsDatePickerVisible = picker.SelectedItem == SD.Period_Custom;
    }
}