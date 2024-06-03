using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using FinTrack_Models;
using System.Net.Http.Json;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.ObjectModel;
using System.Formats.Asn1;
using FinTrack.Services.IServices;

namespace FinTrack.Services
{
    public class RecordApiService : IRecordApiService
    {
        private readonly HttpClient _httpClient;

        public RecordApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ObservableCollection<RecordDTO>> GetDataAsync()
        {
            var response = await _httpClient.GetAsync("/api/Record/GetAll");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var records = System.Text.Json.JsonSerializer.Deserialize<ObservableCollection<RecordDTO>>(json);

            foreach (var record in records)
            {
                if (record.IsIncome)
                {
                    record.Color = "Green";
                }
                else
                {
                    record.Color = "Red";
                }
            }

            return records ?? new ObservableCollection<RecordDTO>();
        }

        public async Task<RecordDTO> CreateRecord(RecordDTO record)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Record/Create", record);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var createdRecord = System.Text.Json.JsonSerializer.Deserialize<RecordDTO>(json);
            if (createdRecord != null)
                return createdRecord;
            return new RecordDTO();
        }

        public async Task<RecordDTO> UpdateRecord(RecordDTO record)
        {
            var response = await _httpClient.PatchAsJsonAsync("/api/Record/Update", record);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var updatedRecord = System.Text.Json.JsonSerializer.Deserialize<RecordDTO>(json);
            if (updatedRecord != null)
                return updatedRecord;
            return new RecordDTO();
        }

        public async Task<RecordDTO> GetRecord(int id)
        {
            var response = await _httpClient.GetAsync("/api/Record/Get/" + id);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var record = System.Text.Json.JsonSerializer.Deserialize<RecordDTO>(json);
            if (record != null)
                return record;
            return new RecordDTO();
        }

        public async Task<int> DeleteRecord(int id)
        {
            var response = await _httpClient.DeleteAsync("/api/Record/Delete/" + id);
            response.EnsureSuccessStatusCode();
            //var json = await response.Content.ReadAsStringAsync();
            //var deletedRecord = JsonConvert.DeserializeObject<int>(json);
            if (response.IsSuccessStatusCode)
                return 1;
            return 0;
        }
    }
}
