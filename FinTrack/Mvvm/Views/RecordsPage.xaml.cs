using FinTrack.Mvvm.ViewModels;

namespace FinTrack.Mvvm.Views;

public partial class RecordsPage : ContentPage
{
	public RecordsPage()
	{
		InitializeComponent();
		BindingContext = new RecordsViewModel();
	}
}