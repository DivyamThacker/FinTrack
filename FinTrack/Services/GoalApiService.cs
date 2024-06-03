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
    public class GoalApiService : IGoalApiService
    {
        private readonly HttpClient _httpClient;

        public GoalApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ObservableCollection<GoalDTO>> GetDataAsync()
        {
            var response = await _httpClient.GetAsync("/api/Goal/GetAll");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var goals = System.Text.Json.JsonSerializer.Deserialize<ObservableCollection<GoalDTO>>(json);

            return goals ?? new ObservableCollection<GoalDTO>();
        }

        public async Task<GoalDTO> CreateGoal(GoalDTO goal)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Goal/Create", goal);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var createdGoal = System.Text.Json.JsonSerializer.Deserialize<GoalDTO>(json);
            if (createdGoal != null)
                return createdGoal;
            return new GoalDTO();
        }

        public async Task<GoalDTO> UpdateGoal(GoalDTO goal)
        {
            var response = await _httpClient.PatchAsJsonAsync("/api/Goal/Update", goal);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var updatedGoal = System.Text.Json.JsonSerializer.Deserialize<GoalDTO>(json);
            if (updatedGoal != null)
                return updatedGoal;
            return new GoalDTO();
        }

        public async Task<GoalDTO> GetGoal(int id)
        {
            var response = await _httpClient.GetAsync("/api/Goal/Get/" + id);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var goal = System.Text.Json.JsonSerializer.Deserialize<GoalDTO>(json);
            if (goal != null)
                return goal;
            return new GoalDTO();
        }

        public async Task<int> DeleteGoal(int id)
        {
            var response = await _httpClient.DeleteAsync("/api/Goal/Delete/" + id);
            response.EnsureSuccessStatusCode();
            //var json = await response.Content.ReadAsStringAsync();
            //var deletedGoal = JsonConvert.DeserializeObject<int>(json);
            if (response.IsSuccessStatusCode)
                return 1;
            return 0;
        }
    }
}
