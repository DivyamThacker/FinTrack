using FinTrack.Helper;
using FinTrack.Services;
using FinTrack.Services.IServices;
using FinTrack_Common;
using FinTrack_Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack.Mvvm.ViewModels
{//[AddINotifyPropertyChangedInterface]
    public class AccountsViewModel : INotifyPropertyChanged
    {
        public string UsernameLabel { get; set; } = default!;
        private readonly IRecordApiService _recordApiService;
        private IMenuHandler _menuHandler;
        public INavigation Navigation { get; set; }
        public ObservableCollection<RecordDTO> Records { get; set; } = new ObservableCollection<RecordDTO>();
        public IEnumerable<string> Categories { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private IPreferences _preferences;
        private INavigation _navigationService;
        public UserDTO User { get; set; }
        public AccountsViewModel(IRecordApiService recordApiService, IMenuHandler menuHandler, IPreferences preferences)
        {
            //this._navigationService = navigation;
            _preferences = preferences;
            var userDetails = _preferences.Get(SD.Local_UserDetails, "null");
            if (userDetails == "null")
            {
                //_navigationService.PushAsync(new BlazorHostPage("Login"));
                Navigation.PushAsync(new BlazorHostPage("Login"));
            }
            else { User = JsonConvert.DeserializeObject<UserDTO>(userDetails); }
            _menuHandler = menuHandler;
            UsernameLabel = User.Name;

            MenuBarHandler.Instance.MenuFlyoutItemClicked += _menuHandler.HandleMenuFlyoutItemClicked;
            _recordApiService = recordApiService;
            Task.Run(async () => await GetRecords());
            Categories = new List<string>
            {
                "All",
                "Transport",
                "Shopping",
                "Bills",
                "Business",
                "Entertainment",
                "Health",
                "Education",
                "Other",
                "Food"
            };
        }
        private async Task GetRecords()
        {
            Records = await _recordApiService.GetDataAsync(User.Id);
        }
        public void Dispose()
        {
            MenuBarHandler.Instance.MenuFlyoutItemClicked -= _menuHandler.HandleMenuFlyoutItemClicked;
        }
    }
}
