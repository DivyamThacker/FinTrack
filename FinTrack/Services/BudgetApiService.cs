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
        private readonly HttpClient _httpClient;

        public BudgetApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ObservableCollection<BudgetDTO>> GetDataAsync()
        {
            var response = await _httpClient.GetAsync("/api/Budget/GetAll");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var budgets = System.Text.Json.JsonSerializer.Deserialize<ObservableCollection<BudgetDTO>>(json);

        
            return budgets ?? new ObservableCollection<BudgetDTO>();
        }

        public async Task<BudgetDTO> CreateBudget(BudgetDTO budget)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Budget/Create", budget);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var createdBudget = System.Text.Json.JsonSerializer.Deserialize<BudgetDTO>(json);
            if (createdBudget != null)
                return createdBudget;
            return new BudgetDTO();
        }

        public async Task<BudgetDTO> UpdateBudget(BudgetDTO budget)
        {
            var response = await _httpClient.PatchAsJsonAsync("/api/Budget/Update", budget);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var updatedBudget = System.Text.Json.JsonSerializer.Deserialize<BudgetDTO>(json);
            if (updatedBudget != null)
                return updatedBudget;
            return new BudgetDTO();
        }

        public async Task<BudgetDTO> GetBudget(int id)
        {
            var response = await _httpClient.GetAsync("/api/Budget/Get/" + id);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var budget = System.Text.Json.JsonSerializer.Deserialize<BudgetDTO>(json);
            if (budget != null)
                return budget;
            return new BudgetDTO();
        }

        public async Task<int> DeleteBudget(int id)
        {
            var response = await _httpClient.DeleteAsync("/api/Budget/Delete/" + id);
            response.EnsureSuccessStatusCode();
            //var json = await response.Content.ReadAsStringAsync();
            //var deletedBudget = JsonConvert.DeserializeObject<int>(json);
            if (response.IsSuccessStatusCode)
                return 1;
            return 0;
        }
    }
}