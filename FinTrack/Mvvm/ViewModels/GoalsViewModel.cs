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

namespace FinTrack.Mvvm.ViewModels
{
    //[AddINotifyPropertyChangedInterface]
    public class GoalsViewModel : INotifyPropertyChanged
    {
        private readonly IGoalApiService _goalApiService;
        private IMenuHandler _menuHandler;
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
        public GoalsViewModel(IGoalApiService goalApiService, IMenuHandler menuHandler)
        {
            _menuHandler = menuHandler;

            MenuBarHandler.Instance.MenuFlyoutItemClicked += _menuHandler.HandleMenuFlyoutItemClicked;
            _goalApiService = _goalApiService;
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
                    NewGoal = new GoalDTO();
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
            NewGoal = new GoalDTO();
            IsFormVisible = false;
            IsListVisible = true;
            IsCreating = false;
            IsUpdating = false;
        }

        private async Task GetGoals()
        {
            Goals = await _goalApiService.GetDataAsync();
            //CalculateChartData();
        }

        public async Task SaveCommandClicked()
        {
            if (NewGoal.Status == null || NewGoal.Status == "")
                NewGoal.Status = SD.Status_Pending;
            if (IsUpdating)
            {
                await _goalApiService.UpdateGoal(NewGoal);
                Goals = await _goalApiService.GetDataAsync();
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
                NewGoal = new GoalDTO();
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
