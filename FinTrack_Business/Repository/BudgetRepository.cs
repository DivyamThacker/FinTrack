using AutoMapper;
using AutoMapper.QueryableExtensions;
using FinTrack_Business.Repository.IRepository;
using FinTrack_Common;
using FinTrack_DataAccess;
using FinTrack_DataAccess.Data;
using FinTrack_Models;
using Microsoft.EntityFrameworkCore;
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
            
            var addedObj = _db.Budgets.Add(obj);
            await _db.SaveChangesAsync();

            return _mapper.Map<Budget, BudgetDTO>(addedObj.Entity);
        }

        public async Task<int> Delete(int id)
        {
            var obj = await _db.Budgets.FirstOrDefaultAsync(u => u.Id == id);
            if (obj != null)
            {
                _db.Budgets.Remove(obj);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<BudgetDTO?> Get(int id)
        {
            var result = await _db.Budgets.Where(x => x.Id == id).ProjectTo<BudgetDTO>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            if (result == null) return new BudgetDTO();
            var records = await _db.Records.Where(x => (x.RecordDate >= result.StartTime) && (x.RecordDate <= result.EndTime) && (!x.IsIncome) && (result.Category == "All" || x.Category == result.Category)).ToListAsync();
            var transactions = await _db.Transactions.Where(x => (x.TransactionDate >= result.StartTime) && (x.TransactionDate <= result.EndTime) && (x.IsUserSender) && (result.Category == "All" || x.Category == result.Category)).ToListAsync();
            result.TotalSpentAmount = records.Sum(x => x.Amount) + transactions.Sum(x => x.Amount);

            if (result.StartTime == DateTime.Now)
                result.DailySpentAmount = 0;
            else
                result.DailySpentAmount = result.TotalSpentAmount / (DateTime.Now - result.StartTime).Days;

            if (result.EndTime <= DateTime.Now || result.Amount < result.TotalSpentAmount)
                result.DailyRecommendedAmount = 0;
            else
            result.DailyRecommendedAmount = (result.Amount - result.TotalSpentAmount) / (result.EndTime - DateTime.Now).Days;
            result.DailyRecommendedAmount = result.DailyRecommendedAmount < 0 ? 0 : result.DailyRecommendedAmount;
            
            if (result.DailySpentAmount == 0 || result.TotalSpentAmount > result.Amount) result.EstimatedDate = DateTime.Now;
            else
            result.EstimatedDate = DateTime.Now.AddDays((result.Amount - result.TotalSpentAmount) / result.DailySpentAmount);
            var ThisWeekRecords = records.Where(x => x.RecordDate >= DateTime.Now.AddDays(-7));
            var ThisWeekTransactions = transactions.Where(x => x.TransactionDate >= DateTime.Now.AddDays(-7));

            result.AmountSpentThisWeek = ThisWeekRecords.Sum(x => x.Amount) + ThisWeekTransactions.Sum(x => x.Amount);

            if (result.TotalSpentAmount >= result.Amount)
            {
                result.Color = "Red";
            }
            else
            {
                result.Color = "Green";
            }
            //logic for status
            if ((result.TotalSpentAmount <= result.Amount) && (result.EndTime > DateTime.Now))
            {
                result.Status = SD.Status_Pending;
            }
            else if ((result.TotalSpentAmount > result.Amount) && (result.EndTime > DateTime.Now))
            {
                result.Status = SD.Status_Busted;
            }
            else if ((result.TotalSpentAmount <= result.Amount) && (result.EndTime <= DateTime.Now))
            {
                result.Status = SD.Status_UnderSpent;
            }
            else if ((result.TotalSpentAmount > result.Amount) && (result.EndTime <= DateTime.Now))
            {
                result.Status = SD.Status_UnderSpent;
            }
            return result;
        }

        public async Task<IEnumerable<BudgetDTO>> GetAll()
        {
            var budgets = await _db.Budgets.ProjectTo<BudgetDTO>(_mapper.ConfigurationProvider).ToListAsync();
            foreach (var budget in budgets)
            {
                var records = await _db.Records.Where(x => (x.RecordDate >= budget.StartTime) && (x.RecordDate <= budget.EndTime) && (!x.IsIncome) && (budget.Category == "All" || x.Category == budget.Category)).ToListAsync();
                var transactions = await _db.Transactions.Where(x => (x.TransactionDate >= budget.StartTime) && (x.TransactionDate <= budget.EndTime) && (x.IsUserSender) && (budget.Category == "All" || x.Category == budget.Category)).ToListAsync();
                budget.TotalSpentAmount = records.Sum(x => x.Amount) + transactions.Sum(x => x.Amount);
                //logic for status
                if ((budget.TotalSpentAmount <= budget.Amount) && (budget.EndTime > DateTime.Now))
                {
                    budget.Status = SD.Status_Pending;
                }
                else if ((budget.TotalSpentAmount > budget.Amount) && (budget.EndTime > DateTime.Now))
                {
                    budget.Status = SD.Status_Busted;
                }
                else if ((budget.TotalSpentAmount <= budget.Amount) && (budget.EndTime <= DateTime.Now))
                {
                    budget.Status = SD.Status_UnderSpent;
                }
                else if ((budget.TotalSpentAmount > budget.Amount) && (budget.EndTime <= DateTime.Now))
                {
                    budget.Status = SD.Status_UnderSpent;
                }
                //color logic
                if (budget.TotalSpentAmount >= budget.Amount)
                {
                    budget.Color = "Red";
                }
                else
                {
                    budget.Color = "Green";
                }
            }
            return budgets;
        }

        public async Task<BudgetDTO> Update(BudgetDTO objDTO)
        {
            var objFromDb = await _db.Budgets.FirstOrDefaultAsync(u => u.Id == objDTO.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = objDTO.Name;
                objFromDb.Description = objDTO.Description;
                objFromDb.Status = objDTO.Status;
                objFromDb.Period = objDTO.Period;
                objFromDb.Category = objDTO.Category;
                objFromDb.Amount = objDTO.Amount;
                objFromDb.Notify = objDTO.Notify;
                objFromDb.StartTime = objDTO.StartTime;
                objFromDb.EndTime = objDTO.EndTime;
                _db.Budgets.Update(objFromDb);
                await _db.SaveChangesAsync();
                return _mapper.Map<Budget, BudgetDTO>(objFromDb);
            }
            return objDTO;
        }
    }
}
