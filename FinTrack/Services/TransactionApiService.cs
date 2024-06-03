using FinTrack.Services.IServices;
using FinTrack_Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack.Services
{
    public class TransactionApiService : ITransactionApiService
    {
        private readonly HttpClient _httpClient;

        public TransactionApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ObservableCollection<TransactionDTO>> GetDataAsync()
        {
            var response = await _httpClient.GetAsync("/api/Transaction/GetAll");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var transactions = System.Text.Json.JsonSerializer.Deserialize<ObservableCollection<TransactionDTO>>(json);

            foreach (var transaction in transactions)
            {
                if (transaction.IsUserSender)
                {
                    transaction.Color = "Red";
                }
                else
                {
                    transaction.Color = "Green";
                }
            }

            return transactions ?? new ObservableCollection<TransactionDTO>();
        }

        public async Task<TransactionDTO> CreateTransaction(TransactionDTO transaction)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Transaction/Create", transaction);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var createdTransaction = System.Text.Json.JsonSerializer.Deserialize<TransactionDTO>(json);
            if (createdTransaction != null)
                return createdTransaction;
            return new TransactionDTO();
        }

        public async Task<TransactionDTO> UpdateTransaction(TransactionDTO transaction)
        {
            var response = await _httpClient.PatchAsJsonAsync("/api/Transaction/Update", transaction);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var updatedTransaction = System.Text.Json.JsonSerializer.Deserialize<TransactionDTO>(json);
            if (updatedTransaction != null)
                return updatedTransaction;
            return new TransactionDTO();
        }

        public async Task<TransactionDTO> GetTransaction(int id)
        {
            var response = await _httpClient.GetAsync("/api/Transaction/Get/" + id);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var transaction = System.Text.Json.JsonSerializer.Deserialize<TransactionDTO>(json);
            if (transaction != null)
                return transaction;
            return new TransactionDTO();
        }

        public async Task<int> DeleteTransaction(int id)
        {
            var response = await _httpClient.DeleteAsync("/api/Transaction/Delete/" + id);
            response.EnsureSuccessStatusCode();
            //var json = await response.Content.ReadAsStringAsync();
            //var deletedTransaction = JsonConvert.DeserializeObject<int>(json);
            if (response.IsSuccessStatusCode)
                return 1;
            return 0;
        }
    }
}
