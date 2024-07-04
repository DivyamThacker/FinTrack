using FinTrack_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack_Business.Repository.IRepository
{
    public interface IGoalRepository
    {
        public Task<GoalDTO> Create(GoalDTO objDTO);
        public Task<GoalDTO> Update(GoalDTO objDTO);
        public Task<int> Delete(int id);
        public Task<GoalDTO>? Get(int id);
        public Task<IEnumerable<GoalDTO>> GetAll(string accountId);
    }
}
