using System.Diagnostics;
using System.Windows.Input;

namespace FinTrack.Mvvm.Views;

public partial class TransactionsPage : ContentPage
{
    public TransactionsPage()
	{
		InitializeComponent();
        BindingContext = this;
	}
    public ICommand NavigationBtnCommand =>
               new Command((text) =>
               {
                   NavigationBtnClicked(text);
               });

    private void NavigationBtnClicked(Object obj)
    {
        //await Navigation.PopAsync();
        var text = (string)obj;
        Debug.WriteLine(text);
    }
}