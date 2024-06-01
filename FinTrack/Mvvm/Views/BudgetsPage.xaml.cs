using FinTrack.Mvvm.ViewModels;
using FinTrack.Services;
using FinTrack.Services.IServices;
using FinTrack_Common;
using FinTrack_Models;

namespace FinTrack.Mvvm.Views;

public partial class BudgetsPage : ContentPage
{
    public BudgetsViewModel MyViewModel { get; private set; }
	public BudgetsPage( )
	{
		InitializeComponent();
        //BindingContext = App.Current.Services.GetService<BudgetsViewModel>();
        //MyViewModel = new BudgetsViewModel();
        //BindingContext = MyViewModel;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        var services = MauiProgram.CreateMauiApp().Services;
        var budgetApiService = services.GetService<IBudgetApiService>();
        MyViewModel = new BudgetsViewModel(budgetApiService);
        BindingContext = MyViewModel;
    }

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