using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FinTrack.Entities;
using FinTrack.Helper;
using FinTrack.Services;
using FinTrack.Services.IServices;
using FinTrack_Common;
using FinTrack_Models;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace FinTrack.Mvvm.ViewModels;

//[AddINotifyPropertyChangedInterface]
public class OverviewViewModel : INotifyPropertyChanged , IDisposable//ObservableObject
{
    private readonly IBudgetApiService _budgetApiService;
    private readonly IRecordApiService _recordApiService;
    private readonly ITransactionApiService _transactionApiService;
    private readonly IGoalApiService _goalApiService;
    public INavigation Navigation { get; set; }
    public ObservableCollection<RecordDTO> Records { get; set; }=new ObservableCollection<RecordDTO>();
    public ObservableCollection<TransactionDTO> Transactions{ get; set; }=new ObservableCollection<TransactionDTO>();
    public ObservableCollection<GoalDTO> Goals { get; set; }=new ObservableCollection<GoalDTO>();
    public ObservableCollection<BudgetDTO> Budgets { get; set; }=new ObservableCollection<BudgetDTO>();
    public int CurrentBalance { get; set; }
    public int IncomeRecords { get; set; }
    public int ExpenseRecords { get; set; }
    public int PositiveTransactions { get; set; }
    public int NegativeTransactions { get; set; }
    public IEnumerable<RecordDTO> ReversedRecords { get; private set; } = new ObservableCollection<RecordDTO>();
    public IEnumerable<TransactionDTO> ReversedTransactions { get; private set; } = new ObservableCollection<TransactionDTO>();

    //public GoalDTO RecentGoal = new GoalDTO();
    private GoalDTO _recentGoal = new GoalDTO();
    public GoalDTO RecentGoal
    {
        get => _recentGoal;
        set
        {
            if (_recentGoal == value) return;
            _recentGoal = value;
            OnPropertyChanged();
        }
    }
    private BudgetDTO _recentBudget =new BudgetDTO();
    public BudgetDTO RecentBudget
    {
        get => _recentBudget;
        set
        {
            if (_recentBudget == value) return;
            _recentBudget = value;
            OnPropertyChanged();
        }
    }

    public IEnumerable<ChartEntity>? Expenses { get; set; } = new ObservableCollection<ChartEntity>();//here I am taking ChartEntity but it can also TransactionDTO
    public IEnumerable<ChartEntity>? Incomes { get; set; } = new ObservableCollection<ChartEntity>();
    public bool IsThisMonthVisible { get; set; } = false;
    public ICommand TimeBtnCommand { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
    //private INavigation _navigationService;
    private IMenuHandler _menuHandler;
    private IPreferences _preferences;
    public UserDTO User { get; set; }
    public string UsernameLabel { get; set; } = default!;

    public OverviewViewModel(IBudgetApiService budgetApiService, IRecordApiService recordApiService, IGoalApiService goalApiService, ITransactionApiService transactionApiService, IMenuHandler menuHandler, IPreferences preferences)
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

        _budgetApiService = budgetApiService;
        _recordApiService = recordApiService;
        _transactionApiService = transactionApiService;
        _goalApiService = goalApiService;
        _menuHandler = menuHandler;

        MenuBarHandler.Instance.MenuFlyoutItemClicked += _menuHandler.HandleMenuFlyoutItemClicked;
        _recentBudget.UserId = User.Id;
        _recentGoal.UserId = User.Id;
        _recentGoal.AccountId = User.AccountId;
        UsernameLabel = User.Name;  
        TimeBtnCommand = new Command((text) =>
        {
            TimeBtnClicked((string)text);
        });
        Task.Run(async () => await GetData());
    }

    private async void TimeBtnClicked(string text)
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
        await GetData();
    }
    private async Task GetData()
    {
        Records = await _recordApiService.GetDataAsync(User.AccountId);
        Transactions = await _transactionApiService.GetDataAsync(User.AccountId);
        Goals = await _goalApiService.GetDataAsync(User.AccountId);
        Budgets = await _budgetApiService.GetDataAsync(User.AccountId);

        //Calculations for frame 1
        CurrentBalance = Records.Where(r => r.IsIncome).Sum(r => r.Amount) - Records.Where(r => !r.IsIncome).Sum(r => r.Amount) + Transactions.Where(r => !r.IsUserSender).Sum(r => r.Amount) - Transactions.Where(r => !r.IsUserSender).Sum(r => r.Amount);

        IncomeRecords = Records.Where(r => r.IsIncome).ToList().Count();
        ExpenseRecords = Records.Count - IncomeRecords;
        PositiveTransactions = Transactions.Where(r => !r.IsUserSender).ToList().Count();
        NegativeTransactions = Transactions.Count - PositiveTransactions;

        ReversedRecords = Records.Where(x => x.RecordDate >= (IsThisMonthVisible ? GetThisMonthStart().Date : GetThisWeekStart().Date)).OrderByDescending(g => g.Id);
        ReversedTransactions = Transactions.Where(x => x.TransactionDate >= (IsThisMonthVisible ? GetThisMonthStart().Date : GetThisWeekStart().Date)).OrderByDescending(g => g.Id);

        RecentGoal = Goals.Where(g => g.Status == SD.Status_Pending).LastOrDefault();
        RecentBudget = Budgets.Where(g => g.Status == SD.Status_Pending).LastOrDefault();

        if (RecentGoal == null)
        {
            RecentGoal = new GoalDTO{UserId = User.Id, AccountId = User.AccountId };
            RecentGoal.Name = "No Goal available";
            RecentGoal.Amount = 1;
            RecentGoal.TotalSavedAmount = 0;
        }
        else { 
        RecentGoal = await _goalApiService.GetGoal(RecentGoal.Id);
        }
        if (RecentBudget == null)
        {
            RecentBudget = new BudgetDTO { UserId = User.Id, AccountId = User.AccountId };
            RecentBudget.Name = "No Budget available";
            RecentBudget.Amount = 1;
            RecentBudget.TotalSpentAmount = 0;
        }
        else
        {
            RecentBudget = await _budgetApiService.GetBudget(RecentBudget.Id);
        }
        CalculateChartData();
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
        Incomes = Records
            .Where(x => x.IsIncome && x.RecordDate >= (IsThisMonthVisible ? GetThisMonthStart().Date : GetThisWeekStart().Date))
            .GroupBy(x => x.RecordDate.Date)
            .Select(g => new ChartEntity
            {
                Date = g.Key,
                Amount = g.Sum(x => x.Amount)
            })
            .ToList();

        var IncomeTransactions = Transactions
            .Where(x => !x.IsUserSender && x.TransactionDate >= (IsThisMonthVisible ? GetThisMonthStart().Date : GetThisWeekStart().Date))
            .GroupBy(x => x.TransactionDate.Date)
            .Select(g => new ChartEntity
            {
                Date = g.Key,
                Amount = g.Sum(x => x.Amount)
            })
            .ToList();

        Incomes = Incomes.Concat(IncomeTransactions).OrderBy(x => x.Date).ToList();

        Expenses = Records
        .Where(x => !x.IsIncome && x.RecordDate >= (IsThisMonthVisible ? GetThisMonthStart().Date : GetThisWeekStart().Date))
        .GroupBy(x => x.RecordDate.Date)
        .Select(g => new ChartEntity
        {
            Date = g.Key,
            Amount = g.Sum(x => x.Amount)
        }).ToList();


        var ExpenseTransactions = Transactions
        .Where(x => x.IsUserSender && x.TransactionDate >= (IsThisMonthVisible ? GetThisMonthStart().Date : GetThisWeekStart().Date))
        .GroupBy(x => x.TransactionDate.Date)
        .Select(g => new ChartEntity
        {
            Date = g.Key,
            Amount = g.Sum(x => x.Amount)
        }).ToList();

        Expenses = Expenses.Concat(ExpenseTransactions).OrderBy(x => x.Date).ToList();
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void Dispose()
    {
        MenuBarHandler.Instance.MenuFlyoutItemClicked -= _menuHandler.HandleMenuFlyoutItemClicked;
    }
}
