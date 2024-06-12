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
    public class TransactionsViewModel : INotifyPropertyChanged
    {
        public string UsernameLabel { get; set; } = default!;
        public INavigation Navigation { get; set; }
        private ITransactionApiService _transactionApiService;
        private IMenuHandler _menuHandler;
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
        public bool IsThisMonthVisible { get; set; } = false;
        public ICommand TimeBtnCommand { get; set; }
        public ICommand SelectedItemCommand { get; set; }
        private IEnumerable<TransactionDTO>? _ThisWeekIncomeTransactions;
        public IEnumerable<TransactionDTO>? ThisWeekIncomeTransactions
        {
            get { return _ThisWeekIncomeTransactions; }
            private set
            {
                _ThisWeekIncomeTransactions = value;
                OnPropertyChanged(nameof(ThisWeekIncomeTransactions));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerable<TransactionDTO>? ThisWeekExpenseTransactions { get; set; }
        public IEnumerable<TransactionDTO>? ThisMonthIncomeTransactions { get; set; }
        public IEnumerable<TransactionDTO>? ThisMonthExpenseTransactions { get; set; }
        private IPreferences _preferences;
        //private INavigation _navigationService;
        public UserDTO User { get; set; }

        public TransactionsViewModel(ITransactionApiService transactionApiService, IMenuHandler menuHandler, IPreferences preferences)
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

            _transactionApiService = transactionApiService;
            MenuBarHandler.Instance.MenuFlyoutItemClicked += _menuHandler.HandleMenuFlyoutItemClicked;
            NewTransaction.UserId = User.Id;
            UsernameLabel = User.Name;
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
            ThisWeekIncomeTransactions = Transactions
            .Where(x => !x.IsUserSender && x.TransactionDate >= GetThisWeekStart().Date)
            .OrderBy(x => x.TransactionDate)
            .GroupBy(x => x.TransactionDate.Date)
            .Select(g => new TransactionDTO
            {
                TransactionDate = g.Key,
                Amount = g.Sum(x => x.Amount)
            });

            ThisWeekExpenseTransactions = Transactions
            .Where(x => x.IsUserSender && x.TransactionDate >= GetThisWeekStart().Date)
            .OrderBy(x => x.TransactionDate)
            .GroupBy(x => x.TransactionDate.Date)
            .Select(g => new TransactionDTO
            {
                TransactionDate = g.Key,
                Amount = g.Sum(x => x.Amount)
            });

            ThisMonthIncomeTransactions = Transactions
            .Where(x => !x.IsUserSender && x.TransactionDate >= GetThisMonthStart().Date)
            .OrderBy(x => x.TransactionDate)
            .GroupBy(x => x.TransactionDate.Date)
            .Select(g => new TransactionDTO
            {
                TransactionDate = g.Key,
                Amount = g.Sum(x => x.Amount)
            });


            ThisMonthExpenseTransactions = Transactions
            .Where(x => x.IsUserSender && x.TransactionDate >= GetThisMonthStart().Date)
            .OrderBy(x => x.TransactionDate)
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
                    IsThisMonthVisible = false;
                    break;
                case "MonthBtn":
                    IsThisMonthVisible = true;
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
                    NewTransaction = new TransactionDTO{UserId = User.Id};
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
            NewTransaction = new TransactionDTO { UserId = User.Id};
            IsFormVisible = false;
            IsListVisible = true;
            IsCreating = false;
            IsUpdating = false;
        }

        private async Task GetTransactions()
        {
            Transactions = await _transactionApiService.GetDataAsync(User.Id);
            CalculateChartData();
        }

        public async Task SaveCommandClicked()
        {
            if (IsUpdating)
            {
                await _transactionApiService.UpdateTransaction(NewTransaction);
                Transactions = await _transactionApiService.GetDataAsync(User.Id);
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
                NewTransaction = new TransactionDTO{ UserId = User.Id };
                IsCreating = false;
                IsFormVisible = false;
                IsListVisible = true;
            }
            CalculateChartData();
        }
        public void Dispose()
        {
            MenuBarHandler.Instance.MenuFlyoutItemClicked -= _menuHandler.HandleMenuFlyoutItemClicked;
        }
    }
}

