using AutoMapper;
using FinTrack;
using FinTrack_Business.Repository.IRepository;
using FinTrack_Common;
using FinTrack_DataAccess;
using FinTrack_DataAccess.Data;
using FinTrack_Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack_Business.Repository
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public BudgetRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<BudgetDTO> Create(BudgetDTO objDTO)
        {
            var obj = _mapper.Map<BudgetDTO, Budget>(objDTO);

            if (obj.Period == SD.Period_OneTime)
            {
                Debug.WriteLine(obj);
            }
            else if (obj.Period == SD.Period_Week)
            {
                obj.StartTime = DateTime.Now;
                obj.EndTime = DateTime.Now;
            }

            var addedObj = _db.Budgets.Add(obj);
            await _db.SaveChangesAsync();

            return _mapper.Map<Budget, BudgetDTO>(addedObj.Entity);
        }

        public Task<int> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BudgetDTO> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BudgetDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<BudgetDTO> Update(BudgetDTO objDTO)
        {
            throw new NotImplementedException();
        }
    }
}
