using FinTrack.IViews;
using FinTrack.Mvvm.ViewModels;
using FinTrack.Services.IServices;

namespace FinTrack.Mvvm.Views.MobileViews;

public partial class MRecordsPage : ContentPage, ISecondPage
{
    public RecordsViewModel MyViewModel { get; private set; }
	public MRecordsPage()
	{
		InitializeComponent();
        //this.BindingContext = new RecordsViewModel(this.Navigation);
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        var services = MauiProgram.CreateMauiApp().Services;
        var recordApiService = services.GetService<IRecordApiService>();
        MyViewModel = new RecordsViewModel(this.Navigation, recordApiService);
        BindingContext = MyViewModel;
    }
}