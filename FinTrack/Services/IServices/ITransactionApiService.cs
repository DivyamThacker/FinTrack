using FinTrack_Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack.Services.IServices
{
    public interface ITransactionApiService
    {
        Task<ObservableCollection<TransactionDTO>> GetDataAsync(string id);
        Task<TransactionDTO> CreateTransaction(TransactionDTO transaction);
        Task<TransactionDTO> UpdateTransaction(TransactionDTO transaction);
        Task<TransactionDTO> GetTransaction(int id);
        Task<int> DeleteTransaction(int id);
    }
}
