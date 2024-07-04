using FinTrack_Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack.Services.IServices
{
    public interface IBudgetApiService
    {
        Task<ObservableCollection<BudgetDTO>> GetDataAsync(string accountId);
        Task<BudgetDTO> CreateBudget(BudgetDTO budget);
        Task<BudgetDTO> UpdateBudget(BudgetDTO budget);
        Task<BudgetDTO> GetBudget(int id);
        Task<int> DeleteBudget(int id);
    }
}
