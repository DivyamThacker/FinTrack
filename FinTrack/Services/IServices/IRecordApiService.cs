using FinTrack_Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack.Services.IServices
{
    public interface IRecordApiService
    {
        Task<ObservableCollection<RecordDTO>> GetDataAsync(string id);
        Task<RecordDTO> CreateRecord(RecordDTO record);
        Task<RecordDTO> UpdateRecord(RecordDTO record);
        Task<RecordDTO> GetRecord(int id);
        Task<int> DeleteRecord(int id);
    }
}
