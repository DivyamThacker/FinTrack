using FinTrack.IViews;
using FinTrack.Mvvm.ViewModels;
using FinTrack.Services;

namespace FinTrack.Mvvm.Views.MobileViews;

public partial class MOverviewPage : ContentPage, IStartPage
{
	public MOverviewPage(BudgetApiService budgetApiService, RecordApiService recordApiService, GoalApiService goalApiService, TransactionApiService transactionApiService)
	{
		InitializeComponent();
        this.BindingContext = new OverviewViewModel(this.Navigation, budgetApiService, recordApiService, goalApiService , transactionApiService);
    }
}