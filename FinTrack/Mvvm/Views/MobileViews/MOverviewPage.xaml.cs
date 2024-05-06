using FinTrack.IViews;
using FinTrack.Mvvm.ViewModels;

namespace FinTrack.Mvvm.Views.MobileViews;

public partial class MOverviewPage : ContentPage, IStartPage
{
	public MOverviewPage()
	{
		InitializeComponent();
        this.BindingContext = new OverviewViewModel(this.Navigation);
    }
}