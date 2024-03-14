using FinTrack_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack_Business.Repository.IRepository
{
    public interface ITransactionRepository
    {
        public Task<TransactionDTO> Create(TransactionDTO objDTO);
        public Task<TransactionDTO> Update(TransactionDTO objDTO);
        public Task<int> Delete(int id);
        public Task<TransactionDTO>? Get(int id);
        public Task<IEnumerable<TransactionDTO>> GetAll();
    }
}
