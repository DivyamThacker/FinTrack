using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
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
using System.Windows.Input;

namespace FinTrack.Mvvm.ViewModels
{
    //[AddINotifyPropertyChangedInterface]
    public class BudgetsViewModel : INotifyPropertyChanged
    {
        public string UsernameLabel { get; set; } = default!;
        private readonly IBudgetApiService _budgetApiService;
        private IMenuHandler _menuHandler;
        public INavigation Navigation { get; set; }
        public BudgetDTO SelectedBudget { get; set; } = default!;
        public bool IsDatePickerVisible { get; set; } = false;
        public ICommand CancelComand { get; set; }
        public ICommand NavigateCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public IEnumerable<string> Periods { get; set; }
        public bool IsPeriodCustom { get; set; } = false;
        public ObservableCollection<BudgetDTO> Budgets { get; set; } = new ObservableCollection<BudgetDTO>();
        public BudgetDTO NewBudget { get; set; } = new BudgetDTO();
        public bool IsListVisible { get; set; } = true;
        public bool IsFormVisible { get; set; } = false;
        public bool IsSelected { get; set; } = false;
        public bool IsCreating { get; set; } = false;
        public bool IsUpdating { get; set; } = false;
        public bool IsThisMonthVisible { get; set; } = false;
        public ICommand TimeBtnCommand { get; set; }
        public ICommand SelectedItemCommand { get; set; }
        private IEnumerable<BudgetDTO>? _ThisWeekBudgets;
        public IEnumerable<BudgetDTO>? ThisWeekBudgets
        {
            get { return _ThisWeekBudgets; }
            private set
            {
                _ThisWeekBudgets = value;
                OnPropertyChanged(nameof(ThisWeekBudgets));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerable<BudgetDTO>? ThisMonthBudgets { get; set; }
        private IPreferences _preferences;
        //private INavigation _navigationService;
        public UserDTO User { get; set; }
        public BudgetsViewModel(IBudgetApiService budgetApiService, IMenuHandler menuHandler, IPreferences preferences)
        {
            //this._navigationService = navigation;
            _preferences = preferences;
            var userDetails = _preferences.Get(SD.Local_UserDetails, "null");
            if (userDetails == "null")
            {
                Navigation.PushAsync(new BlazorHostPage("Login"));
            }
            else { User = JsonConvert.DeserializeObject<UserDTO>(userDetails); }
            _budgetApiService = budgetApiService;
            _menuHandler = menuHandler;

            MenuBarHandler.Instance.MenuFlyoutItemClicked += _menuHandler.HandleMenuFlyoutItemClicked;
            NewBudget.UserId = User.Id;
            UsernameLabel = User.Name;
            Task.Run(async () => await GetBudgets());
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
                OnItemTapped((BudgetDTO)obj);
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
        //    ThisMonthBudgets = Budgets
        //    .Where(x => x.StartTime >= GetThisMonthStart().Date)
        //    .GroupBy(x => x.BudgetDate.Date)
        //    .Select(g => new BudgetDTO
        //    {
        //        BudgetDate = g.Key,
        //        Amount = g.Sum(x => x.Amount)
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



        public async void OnItemTapped(BudgetDTO budget)
        {
            SelectedBudget = await _budgetApiService.GetBudget(budget.Id);
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
                    NewBudget = new BudgetDTO{ UserId = User.Id };
                    break;

                case "Update":
                    if (IsSelected)
                    {
                        IsCreating = false;
                        IsUpdating = true;
                        NewBudget = SelectedBudget;
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
                    NewBudget = SelectedBudget;
                    break;

                case "Delete":
                    if (!IsSelected)
                        App.Current.MainPage.DisplayAlert("Error", "First You need to select the Item to update", "Ok");
                    var result = await App.Current.MainPage.DisplayAlert("Delete", "Are you sure you want to delete this budget?", "Yes", "No");
                    if (result)
                    {
                        IsSelected = false;
                        await _budgetApiService.DeleteBudget(SelectedBudget.Id);
                        //Budgets.Remove(SelectedBudget);
                        await GetBudgets();
                        //CalculateChartData();
                    }
                    break;
            }
        }

        private void CancelCommandClicked()
        {
            NewBudget = new BudgetDTO();
            IsFormVisible = false;
            IsListVisible = true;
            IsCreating = false;
            IsUpdating = false;
        }

        private async Task GetBudgets()
        {
            Budgets = await _budgetApiService.GetDataAsync(User.Id);
            //CalculateChartData();
        }

        public async Task SaveCommandClicked()
        {
            if (NewBudget.Status == null || NewBudget.Status=="")
                NewBudget.Status = SD.Status_Pending;
            NewBudget.Status = SD.Status_Pending;
            if (IsUpdating)
            {
                await _budgetApiService.UpdateBudget(NewBudget);
                Budgets = await _budgetApiService.GetDataAsync(User.Id);
                IsSelected = false;
                IsListVisible = true;
                IsFormVisible = false;
                IsUpdating = false;
            }
            else
            {
                var budget = await _budgetApiService.CreateBudget(NewBudget);
                budget.Color = budget.TotalSpentAmount <= 0 ? "Red" : "Green";
                Budgets.Add(budget);
                NewBudget = new BudgetDTO{ UserId = User.Id };
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
