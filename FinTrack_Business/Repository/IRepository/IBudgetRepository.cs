using FinTrack_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack_Business.Repository.IRepository
{
    public interface IBudgetRepository
    {
        public Task<BudgetDTO> Create(BudgetDTO objDTO);
        public Task<BudgetDTO> Update(BudgetDTO objDTO);
        public Task<int> Delete(int id);
        public Task<BudgetDTO>? Get(int id);
        public Task<IEnumerable<BudgetDTO>> GetAll(string accountId);
    }
}
