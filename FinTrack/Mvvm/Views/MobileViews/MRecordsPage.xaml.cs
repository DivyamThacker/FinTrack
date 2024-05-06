using FinTrack.IViews;
using FinTrack.Mvvm.ViewModels;

namespace FinTrack.Mvvm.Views.MobileViews;

public partial class MRecordsPage : ContentPage, ISecondPage
{
	public MRecordsPage()
	{
		InitializeComponent();
        this.BindingContext = new RecordsViewModel(this.Navigation);
    }
}