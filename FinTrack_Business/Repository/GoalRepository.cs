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
    public class GoalRepository :IGoalRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public GoalRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<GoalDTO> Create(GoalDTO objDTO)
        {
            var obj = _mapper.Map<GoalDTO, Goal>(objDTO);

            var addedObj = _db.Goals.Add(obj);
            await _db.SaveChangesAsync();

            return _mapper.Map<Goal, GoalDTO>(addedObj.Entity);
        }

        public async Task<int> Delete(int id)
        {
            var obj = await _db.Goals.FirstOrDefaultAsync(u => u.Id == id);
            if (obj != null)
            {
                _db.Goals.Remove(obj);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<GoalDTO>? Get(int id)
        {
            var result = await _db.Goals.Where(x => x.Id == id).ProjectTo<GoalDTO>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            if (result == null) return null;
            return result;
        }
        public async Task<IEnumerable<GoalDTO>> GetAll()
        {
            return await _db.Goals.ProjectTo<GoalDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<GoalDTO> Update(GoalDTO objDTO)
        {
            var objFromDb = await _db.Goals.FirstOrDefaultAsync(u => u.Id == objDTO.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = objDTO.Name;
                objFromDb.Description = objDTO.Description;
                objFromDb.Status = objDTO.Status;
                objFromDb.Period = objDTO.Period;
                objFromDb.Category = objDTO.Category;
                objFromDb.GoalAmount = objDTO.GoalAmount;
                objFromDb.StartTime = objDTO.StartTime;
                objFromDb.EndTime = objDTO.EndTime;
                _db.Goals.Update(objFromDb);
                await _db.SaveChangesAsync();
                return _mapper.Map<Goal, GoalDTO>(objFromDb);
            }
            return objDTO;
        }
    }
}
