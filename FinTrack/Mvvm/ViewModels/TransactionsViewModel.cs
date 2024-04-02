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

namespace FinTrack.Mvvm.ViewModels
{
    //[AddINotifyPropertyChangedInterface]
    public class TransactionsViewModel : INotifyPropertyChanged
    {
        private TransactionApiService _transactionApiService;
        public TransactionDTO SelectedTransaction { get; set; } = default!;
        public ICommand CancelComand { get; set; }
        public ICommand NavigateCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public ObservableCollection<TransactionDTO> Transactions { get; set; } = new ObservableCollection<TransactionDTO>();
        public TransactionDTO NewTransaction { get; set; } = new TransactionDTO();
        public bool IsListVisible { get; set; } = true;
        public bool IsFormVisible { get; set; } = false;
        public bool IsSelected { get; set; } = false;
        public bool IsCreating { get; set; } = false;
        public bool IsUpdating { get; set; } = false;
        public bool IsLastMonthVisible { get; set; } = false;
        public ICommand TimeBtnCommand { get; set; }
        public ICommand SelectedItemCommand { get; set; }
        private IEnumerable<TransactionDTO>? _lastWeekIncomeTransactions;
        public IEnumerable<TransactionDTO>? LastWeekIncomeTransactions
        {
            get { return _lastWeekIncomeTransactions; }
            private set
            {
                _lastWeekIncomeTransactions = value;
                OnPropertyChanged(nameof(LastWeekIncomeTransactions));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerable<TransactionDTO>? LastWeekExpenseTransactions { get; set; }
        public IEnumerable<TransactionDTO>? LastMonthIncomeTransactions { get; set; }
        public IEnumerable<TransactionDTO>? LastMonthExpenseTransactions { get; set; }
        public TransactionsViewModel()
        {
            _transactionApiService = new TransactionApiService();
            Task.Run(async () => await GetTransactions());
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
            SelectedItemCommand = new Command((obj) =>
            {
                OnItemTapped((TransactionDTO)obj);
            });
        }


        private DateTime GetLastWeekStart()
        {
            var today = DateTime.Now;
            return today.AddDays(-(int)today.DayOfWeek);//.StartOfWeek(DayOfWeek.Monday)
        }

        private DateTime GetLastMonthStart()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);//.StartOfWeek(DayOfWeek.Monday)
        }

        public void CalculateChartData()
        {
            LastWeekIncomeTransactions = Transactions
            .Where(x => !x.IsUserSender && x.TransactionDate >= GetLastWeekStart().Date)
            .GroupBy(x => x.TransactionDate.Date)
            .Select(g => new TransactionDTO
            {
                TransactionDate = g.Key,
                Amount = g.Sum(x => x.Amount)
            });

            LastWeekExpenseTransactions = Transactions
            .Where(x => x.IsUserSender && x.TransactionDate >= GetLastWeekStart().Date)
            .GroupBy(x => x.TransactionDate.Date)
            .Select(g => new TransactionDTO
            {
                TransactionDate = g.Key,
                Amount = g.Sum(x => x.Amount)
            });

            LastMonthIncomeTransactions = Transactions
            .Where(x => !x.IsUserSender && x.TransactionDate >= GetLastMonthStart().Date)
            .GroupBy(x => x.TransactionDate.Date)
            .Select(g => new TransactionDTO
            {
                TransactionDate = g.Key,
                Amount = g.Sum(x => x.Amount)
            });


            LastMonthExpenseTransactions = Transactions
            .Where(x => x.IsUserSender && x.TransactionDate >= GetLastMonthStart().Date)
            .GroupBy(x => x.TransactionDate.Date)
            .Select(g => new TransactionDTO
            {
                TransactionDate = g.Key,
                Amount = g.Sum(x => x.Amount)
            });
        }
        private void TimeBtnClicked(string text)
        {
            switch (text)
            {
                case "WeekBtn":
                    IsLastMonthVisible = false;
                    break;
                case "MonthBtn":
                    IsLastMonthVisible = true;
                    break;
            }
        }



        public void OnItemTapped(TransactionDTO transaction)
        {
            SelectedTransaction = transaction;
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
                    NewTransaction = new TransactionDTO();
                    break;

                case "Update":
                    if (IsSelected)
                    {
                        IsCreating = false;
                        IsUpdating = true;
                        NewTransaction = SelectedTransaction;
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
                    NewTransaction = SelectedTransaction;
                    break;

                case "Delete":
                    if (!IsSelected)
                        App.Current.MainPage.DisplayAlert("Error", "First You need to select the Item to update", "Ok");
                    var result = await App.Current.MainPage.DisplayAlert("Delete", "Are you sure you want to delete this transaction?", "Yes", "No");
                    if (result)
                    {
                        IsSelected = false;
                        await _transactionApiService.DeleteTransaction(SelectedTransaction.Id);
                        Transactions.Remove(SelectedTransaction);
                        CalculateChartData();
                    }
                    break;
            }
        }

        private void CancelCommandClicked()
        {
            NewTransaction = new TransactionDTO();
            IsFormVisible = false;
            IsListVisible = true;
            IsCreating = false;
            IsUpdating = false;
        }

        private async Task GetTransactions()
        {
            Transactions = await _transactionApiService.GetDataAsync();
            CalculateChartData();
        }

        public async Task SaveCommandClicked()
        {
            if (IsUpdating)
            {
                await _transactionApiService.UpdateTransaction(NewTransaction);
                Transactions = await _transactionApiService.GetDataAsync();
                IsSelected = false;
                IsListVisible = true;
                IsFormVisible = false;
                IsUpdating = false;
            }
            else
            {
                var transaction = await _transactionApiService.CreateTransaction(NewTransaction);
                transaction.Color = transaction.IsUserSender ? "Red" : "Green";
                Transactions.Add(transaction);
                NewTransaction = new TransactionDTO();
                IsCreating = false;
                IsFormVisible = false;
                IsListVisible = true;
            }
            CalculateChartData();
        }
    }
}

