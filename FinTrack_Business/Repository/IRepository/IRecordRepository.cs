using FinTrack_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack_Business.Repository.IRepository
{
    public interface IRecordRepository
    {
        public Task<RecordDTO> Create(RecordDTO objDTO);
        public Task<RecordDTO> Update(RecordDTO objDTO);
        public Task<int> Delete(int id);
        public Task<RecordDTO>? Get(int id);
        public Task<IEnumerable<RecordDTO>> GetAll();
    }
}
