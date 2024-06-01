//using AndroidX.Lifecycle;
using FinTrack.IViews;
using FinTrack.Mvvm.ViewModels;
using FinTrack.Services.IServices;
using FinTrack_Models;

namespace FinTrack.Mvvm.Views;

public partial class RecordsPage : ContentPage, ISecondPage
{
    public RecordsViewModel MyViewModel { get; private set; }
    public RecordsPage()
	{
		InitializeComponent();
        //MyViewModel = new RecordsViewModel(this.Navigation);
        //BindingContext = MyViewModel;
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();

        var services = MauiProgram.CreateMauiApp().Services;
        var recordApiService = services.GetService<IRecordApiService>();
        MyViewModel = new RecordsViewModel(this.Navigation,recordApiService);
        BindingContext = MyViewModel;
    }

    private void RecordsListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item != null)
        {
            MyViewModel.SelectedRecord = (RecordDTO)e.Item;  // Update selected item in view model
            MyViewModel.SelectedItemCommand?.Execute(MyViewModel.SelectedRecord); // Execute the command if available
            // Optional: Perform additional logic with the selected item here
        }
    }
}