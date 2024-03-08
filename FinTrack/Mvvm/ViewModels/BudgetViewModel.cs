using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack.Mvvm.ViewModels
{
    public class BudgetViewModel : ObservableObject
    {
        public List<NavigationItem> NavigationItems { get; set; } = new List<NavigationItem>();
        public BudgetViewModel() 
        {
            GetNavigationItems();
        }
        private void GetNavigationItems()
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

}
