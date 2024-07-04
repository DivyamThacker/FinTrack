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
            if (result == null) return new GoalDTO();
            var records = await _db.Records.Where(x => (x.RecordDate >= result.StartTime) && (x.RecordDate <= result.EndTime) && (result.Category == "All" || x.Category == result.Category)).ToListAsync();

            var transactions = await _db.Transactions.Where(x => (x.TransactionDate >= result.StartTime) && (x.TransactionDate <= result.EndTime) && (result.Category == "All" || x.Category == result.Category)).ToListAsync();
            result.TotalSavedAmount = records.Where(x=>x.IsIncome).Sum(x => x.Amount) - records.Where(x => !x.IsIncome).Sum(x => x.Amount) + transactions.Where(x => !x.IsUserSender).Sum(x => x.Amount) - transactions.Where(x => x.IsUserSender).Sum(x => x.Amount);
            if  (result.StartTime == DateTime.Now)
                  result.DailySavedAmount = 0;
            else
                result.DailySavedAmount = result.TotalSavedAmount / (DateTime.Now - result.StartTime).Days;

            if (result.EndTime <= DateTime.Now || result.Amount > result.TotalSavedAmount)
                result.DailyRecommendedAmount = 0;
            else
            result.DailyRecommendedAmount = (result.Amount- result.TotalSavedAmount) / (result.EndTime - DateTime.Now).Days;
            
            if (result.DailySavedAmount ==0 || result.TotalSavedAmount <= 0)   result.EstimatedDate = DateTime.Now;
            else
            result.EstimatedDate = DateTime.Now.AddDays((result.Amount - result.TotalSavedAmount) / result.DailySavedAmount);
            var ThisWeekRecords = records.Where(x => x.RecordDate >= DateTime.Now.AddDays(-7));
            var ThisWeekTransactions = transactions.Where(x => x.TransactionDate >= DateTime.Now.AddDays(-7));

            result.AmountSavedThisWeek = ThisWeekRecords.Where(x => x.IsIncome).Sum(x => x.Amount) - ThisWeekRecords.Where(x => !x.IsIncome).Sum(x => x.Amount) + ThisWeekTransactions.Where(x => !x.IsUserSender).Sum(x => x.Amount) - ThisWeekTransactions.Where(x => x.IsUserSender).Sum(x => x.Amount);

            if (result.TotalSavedAmount > result.Amount)
            {
                result.Color = "Green";
            }
            else
            {
                result.Color = "Red";
            }
            //logic for status
            if (result.TotalSavedAmount >=result.Amount)
            {
                result.Status = SD.Status_Achieved;
            }
            else if (result.EndTime <= DateTime.Now)
            {
                result.Status =SD.Status_Failed;
            }
            else
            {
                result.Status = SD.Status_Pending;
            }


            return result;
        }
        public async Task<IEnumerable<GoalDTO>> GetAll(string accountId)
        {
            var goals = await _db.Goals.ProjectTo<GoalDTO>(_mapper.ConfigurationProvider).Where(goal=> goal.AccountId == accountId).ToListAsync();
            foreach (var goal in goals)
            {
                var records = await _db.Records.Where(x =>(x.AccountId == accountId) && (x.RecordDate >= goal.StartTime) && (x.RecordDate <= goal.EndTime) && (goal.Category == "All" || x.Category == goal.Category)).ToListAsync();
                var transactions = await _db.Transactions.Where(x => (x.AccountId == accountId) && (x.TransactionDate >= goal.StartTime) && (x.TransactionDate <= goal.EndTime) && (goal.Category == "All" || x.Category == goal.Category)).ToListAsync();
                goal.TotalSavedAmount = records.Where(x => x.IsIncome).Sum(x => x.Amount) - records.Where(x => !x.IsIncome).Sum(x => x.Amount) + transactions.Where(x => !x.IsUserSender).Sum(x => x.Amount) - transactions.Where(x => x.IsUserSender).Sum(x => x.Amount);
                //logic for status
                if (goal.TotalSavedAmount >= goal.Amount)
                {
                    goal.Status = SD.Status_Achieved;
                }
                else if (goal.EndTime <= DateTime.Now)
                {
                    goal.Status = SD.Status_Failed;
                }
                else
                {
                    goal.Status = SD.Status_Pending;
                }
                //color logic
                if (goal.TotalSavedAmount < goal.Amount)
                {
                    goal.Color = "Red";
                }
                else
                {
                    goal.Color = "Green";
                }
            }
            return goals;
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
                objFromDb.Amount = objDTO.Amount;
                objFromDb.Notify = objDTO.Notify;
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
