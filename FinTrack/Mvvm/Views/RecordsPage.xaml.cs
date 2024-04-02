//using AndroidX.Lifecycle;
using FinTrack.Mvvm.ViewModels;
using FinTrack_Models;

namespace FinTrack.Mvvm.Views;

public partial class RecordsPage : ContentPage
{

	public RecordsPage()
	{
		InitializeComponent();
        MyViewModel = new RecordsViewModel();
        BindingContext = MyViewModel;
	}

    public RecordsViewModel MyViewModel { get; private set; }

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