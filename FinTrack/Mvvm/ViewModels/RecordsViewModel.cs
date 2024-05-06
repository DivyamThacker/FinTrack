using FinTrack.Services;
using FinTrack_Models;
//using Microsoft.Maui.Controls.Compatibility.Platform.UWP;
using PropertyChanged;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FinTrack.Converters;
using FinTrack.Entities;
using System.ComponentModel;
namespace FinTrack.Mvvm.ViewModels
{
//[AddINotifyPropertyChangedInterface]
    public class RecordsViewModel : INotifyPropertyChanged
    {
        private RecordApiService _recordApiService;
        public RecordDTO SelectedRecord { get; set; } = default!;
        public ICommand CancelComand { get; set; }
        public ICommand NavigateCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public ObservableCollection<RecordDTO> Records { get; set; } =  new ObservableCollection<RecordDTO>();
        public RecordDTO NewRecord { get; set; } = new RecordDTO();
        public bool IsListVisible { get; set; } = true;
        public bool IsFormVisible { get; set; } = false;
        public bool IsSelected { get; set; } = false;
        public bool IsCreating { get; set; } = false;
        public bool IsUpdating { get; set; } = false;
        public bool IsThisMonthVisible { get; set; } = false;
        public ICommand TimeBtnCommand { get; set; }
        public ICommand SelectedItemCommand { get; set; }
        private IEnumerable<RecordDTO>? _ThisWeekIncomeRecords;
        public IEnumerable<RecordDTO>? ThisWeekIncomeRecords
        {
            get { return _ThisWeekIncomeRecords; }
            private set
            {
                _ThisWeekIncomeRecords = value;
                OnPropertyChanged(nameof(ThisWeekIncomeRecords)); 
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerable<RecordDTO>? ThisWeekExpenseRecords { get; set; }
        public IEnumerable<RecordDTO>? ThisMonthIncomeRecords { get; set; }
        public IEnumerable<RecordDTO>? ThisMonthExpenseRecords { get; set; }
        private INavigation _navigationService;
        public RecordsViewModel(INavigation navigation)
        {
            this._navigationService = navigation;
            _recordApiService = new RecordApiService();
            Task.Run(async () => await GetRecords());
            CancelComand = new Command( () =>
            {
                CancelCommandClicked();
            });
            SaveCommand = new Command(async () =>
            {
                await SaveCommandClicked();
            });
            NavigateCommand = new Command( (text) =>
            {
                 NavigateClicked((string)text);
            });
            TimeBtnCommand = new Command( (text) =>
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
            SelectedItemCommand =  new Command((obj) =>
            {
                OnItemTapped((RecordDTO)obj);
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

        public void CalculateChartData()
        {
            //ThisWeekIncomeRecords =  Records.Where(x => x.IsIncome && x.RecordDate >= GetThisWeekStart().Date);
            ThisWeekIncomeRecords = Records
            .Where(x => x.IsIncome && x.RecordDate >= GetThisWeekStart().Date)
            .OrderBy(x => x.RecordDate)
            .GroupBy(x => x.RecordDate.Date) // Group by the date part of the RecordDate
            .Select(g => new RecordDTO
            {
                RecordDate = g.Key,
                Amount = g.Sum(x => x.Amount)
            });

            ThisWeekExpenseRecords = Records
            .Where(x => !x.IsIncome && x.RecordDate >= GetThisWeekStart().Date)
            .OrderBy(x => x.RecordDate)
            .GroupBy(x => x.RecordDate.Date) 
            .Select(g => new RecordDTO
            {
                RecordDate = g.Key,
                Amount = g.Sum(x => x.Amount)
            });

            ThisMonthIncomeRecords = Records
            .Where(x => x.IsIncome && x.RecordDate >= GetThisMonthStart().Date)
            .OrderBy(x => x.RecordDate)
            .GroupBy(x => x.RecordDate.Date) 
            .Select(g => new RecordDTO
            {
                RecordDate = g.Key,
                Amount = g.Sum(x => x.Amount)
            });

            ThisMonthExpenseRecords = Records
            .Where(x => !x.IsIncome && x.RecordDate >= GetThisMonthStart().Date)
            .OrderBy(x => x.RecordDate)
            .GroupBy(x => x.RecordDate.Date) 
            .Select(g => new RecordDTO
            {
                RecordDate = g.Key,
                Amount = g.Sum(x => x.Amount)
            });
        }
        private void TimeBtnClicked(string text)
        {
            switch(text)
            {
                case "WeekBtn":
                    IsThisMonthVisible = false;
                    break;
                case "MonthBtn":
                    IsThisMonthVisible = true;
                    break;
            }
        }



        public void OnItemTapped(RecordDTO record)
        {
            SelectedRecord = record;
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
                    NewRecord = new RecordDTO();
                    break;

                case "Update":
                    if (IsSelected)
                    {
                        IsCreating = false; 
                        IsUpdating = true;
                        NewRecord = SelectedRecord;
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
                    NewRecord = SelectedRecord;
                    break;

                case "Delete":
                    if (!IsSelected)
                    App.Current.MainPage.DisplayAlert("Error", "First You need to select the Item to update", "Ok");
                    var result = await App.Current.MainPage.DisplayAlert("Delete", "Are you sure you want to delete this record?", "Yes", "No");
                    if (result)
                    {
                        IsSelected = false;
                        await _recordApiService.DeleteRecord(SelectedRecord.Id);
                        Records.Remove(SelectedRecord);
                        CalculateChartData();
                    }
                    break;
            }
        }

        private void CancelCommandClicked()
        {
            NewRecord = new RecordDTO();
            IsFormVisible = false;
            IsListVisible = true;
            IsCreating = false;
            IsUpdating = false;
        }

        private async Task GetRecords()
        {
            Records = await _recordApiService.GetDataAsync();
            CalculateChartData();
        }

        public async Task SaveCommandClicked()
        {
            if (IsUpdating)
            {
                await _recordApiService.UpdateRecord(NewRecord);
                Records = await _recordApiService.GetDataAsync();
                IsSelected = false;
                IsListVisible = true;
                IsFormVisible = false;
                IsUpdating = false;
            }
            else
            {
                var record = await _recordApiService.CreateRecord(NewRecord);
                record.Color = record.IsIncome ? "Green" : "Red";
                Records.Add(record);
                NewRecord = new RecordDTO();
                IsCreating = false;
                IsFormVisible = false;
                IsListVisible = true;
            }
            CalculateChartData();
        }
    }
}
