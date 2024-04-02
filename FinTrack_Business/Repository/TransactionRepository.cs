using AutoMapper;
using FinTrack_Business.Repository.IRepository;
using FinTrack_DataAccess.Data;
using FinTrack_DataAccess;
using FinTrack_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

namespace FinTrack_Business.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public TransactionRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<TransactionDTO> Create(TransactionDTO objDTO)
        {
            var obj = _mapper.Map<TransactionDTO, Transaction>(objDTO);

            var addedObj = _db.Transactions.Add(obj);
            await _db.SaveChangesAsync();

            return _mapper.Map<Transaction, TransactionDTO>(addedObj.Entity);
        }

        public async Task<int> Delete(int id)
        {
            var obj = await _db.Transactions.FirstOrDefaultAsync(u => u.Id == id);
            if (obj != null)
            {
                _db.Transactions.Remove(obj);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<TransactionDTO>? Get(int id)
        {
            var result = await _db.Transactions.Where(x => x.Id == id).ProjectTo<TransactionDTO>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            if (result == null) return null;
            return result;
        }

        public async Task<IEnumerable<TransactionDTO>> GetAll()
        {
            return await _db.Transactions.ProjectTo<TransactionDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<TransactionDTO> Update(TransactionDTO objDTO)
        {
            var objFromDb = await _db.Transactions.FirstOrDefaultAsync(u => u.Id == objDTO.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = objDTO.Name;
                objFromDb.Description = objDTO.Description;
                objFromDb.Amount = objDTO.Amount;
                objFromDb.TransactionDate = objDTO.TransactionDate;
                objFromDb.SenderUsername = objDTO.SenderUsername;
                objFromDb.RecieverUsername = objDTO.RecieverUsername;
                objFromDb.IsUserSender = objDTO.IsUserSender;
                objFromDb.Category = objDTO.Category;
                _db.Transactions.Update(objFromDb);
                await _db.SaveChangesAsync();
                return _mapper.Map<Transaction, TransactionDTO>(objFromDb);
            }
            return objDTO;
        }
    }
}
