using FinTrack_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack.Services.IServices
{
    public interface IAccountService
    {
        Task<AccountCreationResponseDTO>CreateAccountAsync(AccountCreationRequestDTO accountCreationRequestDTO);
        Task<int> DeleteAccountAsync(string accountId);
        Task<List<AccountDTO>> GetAccounts(string userId);
    }
}
