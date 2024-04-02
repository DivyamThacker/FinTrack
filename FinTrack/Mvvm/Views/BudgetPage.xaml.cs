using FinTrack.Mvvm.ViewModels;

namespace FinTrack.Mvvm.Views;

public partial class BudgetPage : ContentPage
{
	public BudgetPage()
	{
		InitializeComponent();
		BindingContext = new BudgetsViewModel();
	}
}