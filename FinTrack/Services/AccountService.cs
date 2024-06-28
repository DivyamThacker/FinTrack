using FinTrack.Services.IServices;
using FinTrack_Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace FinTrack.Services
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient _httpClient;
        public AccountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<AccountCreationResponseDTO> CreateAccountAsync(AccountCreationRequestDTO accountRequest)
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/Account/CreateAccountAsync/", accountRequest);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var accountCreationResponseDTO = JsonSerializer.Deserialize<AccountCreationResponseDTO>(json);
            Debug.WriteLine(accountCreationResponseDTO?.AccountId);
            return accountCreationResponseDTO;
        }

        public async Task<int> DeleteAccountAsync(string accountId)
        {
            var response = await _httpClient.DeleteAsync($"/api/Account/DeleteAccount/{accountId}");
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode) return 1;
            return 0;
        }

        public async Task<List<AccountDTO>> GetAccounts(string userId)
        {
            var response = await _httpClient.GetAsync($"/api/Account/GetAccounts/{userId}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var accounts = JsonSerializer.Deserialize<List<AccountDTO>>(json);
            return accounts;
        }
    }
}
