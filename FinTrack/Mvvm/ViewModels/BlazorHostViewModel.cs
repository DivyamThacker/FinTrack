using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FinTrack.Components.Pages.Account;
using FinTrack.Components.Pages;

namespace FinTrack.Mvvm.ViewModels
{
    public class BlazorHostViewModel : INotifyPropertyChanged
    {
        //private Type _blazorPage;
        private string _pageName;

        //public Type BlazorPage
        //{
        //    get => _blazorPage;
        //    private set
        //    {
        //        if (_blazorPage != value)
        //        {
        //            _blazorPage = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        public string PageName
        {
            get => _pageName;
            set
            {
                if (_pageName != value)
                {
                    _pageName = value;
                    OnPropertyChanged();
                    UpdateBlazorPage();
                }
            }
        }

        public BlazorHostViewModel()
        {
            // Set a default page name
            PageName = "login"; // Default page name
        }

        private void UpdateBlazorPage()
        {
            //BlazorPage = PageName switch
            //{
            //    "Login" => typeof(Login),
            //    "Dashboard" => typeof(Dashboard),
            //    "Counter"=>typeof(Counter),
            //    "Account"=>typeof(Account),
            //    _ => typeof(Login) // Handle unknown pages
            //};

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}