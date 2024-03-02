using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FinTrack_DataAccess;
using PropertyChanged;
using System.Diagnostics;
using System.Windows.Input;

namespace FinTrack.Mvvm.ViewModels;

[AddINotifyPropertyChangedInterface]
public class OverviewViewModel : ObservableObject
{
    public List<NavigationItem> NavigationItems { get; set; } = new List<NavigationItem>();
    //public ICommand? NavigationBtnCommand { get; }

    public OverviewViewModel()
	{
        GetNavigationItems();
        
    }

    //public static ICommand SearchCommand =>
    //           new Command( (searchText) =>
    //           {
    //               Debug.WriteLine(searchText);
    //           });

    public void GetNavigationItems()
    {
        NavigationItems.Add(new NavigationItem { Glyph = "\uf0e4", Text = "Overview" });
        NavigationItems.Add(new NavigationItem { Glyph = "\uf0ec", Text = "Transactions" });
        NavigationItems.Add(new NavigationItem { Glyph = "\uf0f2", Text = "Accounts" });
        NavigationItems.Add(new NavigationItem { Glyph = "\uf155", Text = "Budget" });
        NavigationItems.Add(new NavigationItem { Glyph = "\ue816", Text = "Goals" });
        NavigationItems.Add(new NavigationItem { Glyph = "\ue80a", Text = "Settings" });
        foreach (var item in NavigationItems)
        {
            item.NavigationBtnCommand = new Command((text) => 
            { 
                WeakReferenceMessenger.Default.Send(new MyMessage((string)text)); 
            });
        }
    }
}
