using FinTrack.Services;
using FinTrack_Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FinTrack_Common;
using FinTrack.Services.IServices;
using FinTrack.Helper;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
//using Windows.Security.Authentication.OnlineId;
//using Windows.System;

namespace FinTrack.Mvvm.ViewModels
{
    //[AddINotifyPropertyChangedInterface]
    public class GoalsViewModel : INotifyPropertyChanged
    {
        private readonly IGoalApiService _goalApiService;
        public string UsernameLabel { get; set; } = default!;
        private IMenuHandler _menuHandler;
        public INavigation Navigation { get; set; }
        public GoalDTO SelectedGoal { get; set; } = default!;
        public bool IsDatePickerVisible { get; set; } = false;
        public ICommand CancelComand { get; set; }
        public ICommand NavigateCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public IEnumerable<string> Periods { get; set; }
        public bool IsPeriodCustom { get; set; } = false;
        public ObservableCollection<GoalDTO> Goals { get; set; } = new ObservableCollection<GoalDTO>();
        public GoalDTO NewGoal { get; set; } = new GoalDTO();
        public bool IsListVisible { get; set; } = true;
        public bool IsFormVisible { get; set; } = false;
        public bool IsSelected { get; set; } = false;
        public bool IsCreating { get; set; } = false;
        public bool IsUpdating { get; set; } = false;
        public bool IsThisMonthVisible { get; set; } = false;
        public ICommand TimeBtnCommand { get; set; }
        public ICommand SelectedItemCommand { get; set; }
        private IEnumerable<GoalDTO>? _ThisWeekGoals;
        public IEnumerable<GoalDTO>? ThisWeekGoals
        {
            get { return _ThisWeekGoals; }
            private set
            {
                _ThisWeekGoals = value;
                OnPropertyChanged(nameof(ThisWeekGoals));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerable<GoalDTO>? ThisMonthGoals { get; set; }
        private IPreferences _preferences;
        //private INavigation _navigationService;
        public UserDTO User { get; set; }
        public GoalsViewModel(IGoalApiService goalApiService, IMenuHandler menuHandler, IPreferences preferences)
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
            _goalApiService = goalApiService;
            MenuBarHandler.Instance.MenuFlyoutItemClicked += _menuHandler.HandleMenuFlyoutItemClicked;
            NewGoal.UserId = User.Id;
            NewGoal.AccountId = User.AccountId;
            UsernameLabel = User.Name;

            Task.Run(async () => await GetGoals());
            CancelComand = new Command(() =>
            {
                CancelCommandClicked();
            });
            SaveCommand = new Command(async () =>
            {
                await SaveCommandClicked();
            });
            NavigateCommand = new Command((text) =>
            {
                NavigateClicked((string)text);
            });
            TimeBtnCommand = new Command((text) =>
            {
                TimeBtnClicked((string)text);
            });
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
            Periods = new List<string>
            {
                SD.Period_Week,SD.Period_Month,SD.Period_Year,SD.Period_Custom
            };
            SelectedItemCommand = new Command((obj) =>
            {
                OnItemTapped((GoalDTO)obj);
            });
        }


        private DateTime GetThisWeekStart()
        {
            var today = DateTime.Now;
            return today.AddDays(-(int)today.DayOfWeek);//.StartOfWeek(DayOfWeek.Monday)
        }

        private DateTime GetThisMonthStart()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);//.StartOfWeek(DayOfWeek.Monday)
        }

        //public void CalculateChartData()
        //{
        //    ThisMonthGoals = Goals
        //    .Where(x => x.StartTime >= GetThisMonthStart().Date)
        //    .GroupBy(x => x.GoalDate.Date)
        //    .Select(g => new GoalDTO
        //    {
        //        GoalDate = g.Key,
        //        Amount = g.Sum(x => x.Amount)
        //        AccountId = User.AccountId
        //    });
        //}
        private void TimeBtnClicked(string text)
        {
            switch (text)
            {
                case "WeekBtn":
                    IsThisMonthVisible = false;
                    break;
                case "MonthBtn":
                    IsThisMonthVisible = true;
                    break;
            }
        }



        public async void OnItemTapped(GoalDTO goal)
        {
            SelectedGoal = await _goalApiService.GetGoal(goal.Id);
            IsSelected = true;
        }

        private async void NavigateClicked(string text)
        {
            switch (text)
            {
                case "Create":
                    IsSelected = false;
                    IsCreating = true;
                    IsUpdating = false;
                    IsFormVisible = true;
                    IsListVisible = false;
                    NewGoal = new GoalDTO{ UserId = User.Id, AccountId = User.AccountId };
                    break;

                case "Update":
                    if (IsSelected)
                    {
                        IsCreating = false;
                        IsUpdating = true;
                        NewGoal = SelectedGoal;
                        IsFormVisible = true;
                        IsListVisible = false;
                    }
                    else
                    {
                        App.Current.MainPage.DisplayAlert("Error", "First You need to select the Item to update", "Ok");
                    }
                    break;

                case "View":
                    IsCreating = false;
                    IsUpdating = false;
                    IsFormVisible = false;
                    IsListVisible = true;
                    IsSelected = false;
                    NewGoal = SelectedGoal;
                    break;

                case "Delete":
                    if (!IsSelected)
                        App.Current.MainPage.DisplayAlert("Error", "First You need to select the Item to update", "Ok");
                    var result = await App.Current.MainPage.DisplayAlert("Delete", "Are you sure you want to delete this goal?", "Yes", "No");
                    if (result)
                    {
                        IsSelected = false;
                        await _goalApiService.DeleteGoal(SelectedGoal.Id);
                        //Goals.Remove(SelectedGoal);
                        await GetGoals();
                        //CalculateChartData();
                    }
                    break;
            }
        }

        private void CancelCommandClicked()
        {
            NewGoal = new GoalDTO { UserId = User.Id, AccountId = User.AccountId };
            IsFormVisible = false;
            IsListVisible = true;
            IsCreating = false;
            IsUpdating = false;
        }

        private async Task GetGoals()
        {
            Goals = await _goalApiService.GetDataAsync(User.AccountId);
            //CalculateChartData();
        }

        public async Task SaveCommandClicked()
        {
            if (NewGoal.Status == null || NewGoal.Status == "")
                NewGoal.Status = SD.Status_Pending;
            if (IsUpdating)
            {
                await _goalApiService.UpdateGoal(NewGoal);
                Goals = await _goalApiService.GetDataAsync(User.AccountId);
                IsSelected = false;
                IsListVisible = true;
                IsFormVisible = false;
                IsUpdating = false;
            }
            else
            {
                var goal = await _goalApiService.CreateGoal(NewGoal);
                goal.Color = goal.TotalSavedAmount<=0 ? "Red" : "Green";
                Goals.Add(goal);
                NewGoal = new GoalDTO{UserId = User.Id , AccountId = User.AccountId };
                IsCreating = false;
                IsFormVisible = false;
                IsListVisible = true;
            }
            //CalculateChartData();
        }
            public void Dispose()
            {
                MenuBarHandler.Instance.MenuFlyoutItemClicked -= _menuHandler.HandleMenuFlyoutItemClicked;
            }
    }
}
