using FinTrack_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack_Business.Repository.IRepository
{
    public interface IAccountRepository
    {
        Task<AccountCreationResponseDTO> Create(AccountCreationRequestDTO accountRequest);
        Task<int> DeleteAccount(string accountId);
        List<AccountDTO> GetAllAccounts(string userId);
    }
}
