using FinTrack_Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack.Services.IServices
{
    public interface IGoalApiService
    {
        Task<ObservableCollection<GoalDTO>> GetDataAsync(string accountId);
        Task<GoalDTO> CreateGoal(GoalDTO goal);
        Task<GoalDTO> UpdateGoal(GoalDTO goal);
        Task<GoalDTO> GetGoal(int id);
        Task<int> DeleteGoal(int id);
    }
}
