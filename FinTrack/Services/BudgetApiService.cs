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

namespace FinTrack.Services
{
    public class BudgetApiService : IBudgetApiService
    {
        private string ApiUrl = "https://localhost:7263/api/Budget/";
        private readonly HttpClient _httpClient;

        public BudgetApiService(HttpClient MyNamedHttpClient)
        {
            _httpClient = MyNamedHttpClient;
        }

        public async Task<ObservableCollection<BudgetDTO>> GetDataAsync()
        {
            Debug.WriteLine(_httpClient.BaseAddress);
            var response = await _httpClient.GetAsync(ApiUrl+ "GetAll");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var budgets = System.Text.Json.JsonSerializer.Deserialize<ObservableCollection<BudgetDTO>>(json);

        
            return budgets ?? new ObservableCollection<BudgetDTO>();
        }

        public async Task<BudgetDTO> CreateBudget(BudgetDTO budget)
        {
            var response = await _httpClient.PostAsJsonAsync(ApiUrl + "Create", budget);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var createdBudget = System.Text.Json.JsonSerializer.Deserialize<BudgetDTO>(json);
            if (createdBudget != null)
                return createdBudget;
            return new BudgetDTO();
        }

        public async Task<BudgetDTO> UpdateBudget(BudgetDTO budget)
        {
            var response = await _httpClient.PatchAsJsonAsync(ApiUrl + "Update", budget);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var updatedBudget = System.Text.Json.JsonSerializer.Deserialize<BudgetDTO>(json);
            if (updatedBudget != null)
                return updatedBudget;
            return new BudgetDTO();
        }

        public async Task<BudgetDTO> GetBudget(int id)
        {
            var response = await _httpClient.GetAsync(ApiUrl + "Get/" + id);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var budget = System.Text.Json.JsonSerializer.Deserialize<BudgetDTO>(json);
            if (budget != null)
                return budget;
            return new BudgetDTO();
        }

        public async Task<int> DeleteBudget(int id)
        {
            var response = await _httpClient.DeleteAsync(ApiUrl + "Delete/" + id);
            response.EnsureSuccessStatusCode();
            //var json = await response.Content.ReadAsStringAsync();
            //var deletedBudget = JsonConvert.DeserializeObject<int>(json);
            if (response.IsSuccessStatusCode)
                return 1;
            return 0;
        }
    }
}