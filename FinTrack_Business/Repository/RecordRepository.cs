using AutoMapper;
using AutoMapper.QueryableExtensions;
using FinTrack_Business.Repository.IRepository;
using FinTrack_DataAccess;
using FinTrack_DataAccess.Data;
using FinTrack_Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack_Business.Repository
{
    public class RecordRepository : IRecordRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public RecordRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<RecordDTO> Create(RecordDTO objDTO)
        {
            var obj = _mapper.Map<RecordDTO, Record>(objDTO);

            var addedObj = _db.Records.Add(obj);
            await _db.SaveChangesAsync();

            return _mapper.Map<Record, RecordDTO>(addedObj.Entity);
        }

        public async Task<int> Delete(int id)
        {
             var obj = await _db.Records.FirstOrDefaultAsync(u => u.Id == id);
            if (obj != null)
            {
                _db.Records.Remove(obj);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<RecordDTO>? Get(int id)
        {
            var result = await _db.Records.Where(x => x.Id == id).ProjectTo<RecordDTO>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            if (result == null) return null;
            return result;
        }

        public async Task<IEnumerable<RecordDTO>> GetAll()
        {
            return await _db.Records.ProjectTo<RecordDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<RecordDTO> Update(RecordDTO objDTO)
        {
            var objFromDb = await _db.Records.FirstOrDefaultAsync(u => u.Id == objDTO.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = objDTO.Name;
                objFromDb.Description = objDTO.Description;
                objFromDb.Amount = objDTO.Amount;
                objFromDb.Category = objDTO.Category;
                objFromDb.IsIncome = objDTO.IsIncome;
                _db.Records.Update(objFromDb);
                await _db.SaveChangesAsync();
                return _mapper.Map<Record, RecordDTO>(objFromDb);
            }
            return objDTO;
        }
    }
}
