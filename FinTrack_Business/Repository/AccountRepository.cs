using AutoMapper;
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
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public AccountRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<AccountCreationResponseDTO> Create(AccountCreationRequestDTO accountRequest)
        {
            if (accountRequest == null)
            {
                   return new AccountCreationResponseDTO
                   {
                    Success = false,
                    Message = "Account creation request is empty"
                };
            }
            if (accountRequest.UserId == null)
            {
                return new AccountCreationResponseDTO
                {
                    Success = false,
                    Message = "User ID is required"
                };
            }
            var user = _db.ApplicationUsers.Where(u => u.Id == accountRequest.UserId ).FirstOrDefault();
            if (user == null)
            {
                return new AccountCreationResponseDTO
                {
                    Success = false,
                    Message = "User does not exist"
                };
            }
            if (user.Accounts != null)
            {
                foreach (var account in user.Accounts) {
                    if (account.Type == accountRequest.AccountType && account.Name == accountRequest.AccountName)
                    {
                        return new AccountCreationResponseDTO
                        {
                            Success = false,
                            Message = "Account with the same name already exists"
                        };
                    }
                }
            }

            var obj = new Account
            {
                Name = accountRequest.AccountName,
                Type = accountRequest.AccountType,
                UserId = accountRequest.UserId,
                User = _db.ApplicationUsers.FirstOrDefault(u => u.Id == accountRequest.UserId)
            };
            var addedObj = _db.Accounts.Add(obj);

            //var obj = _mapper.Map<AccountCreationRequestDTO, Account>(accountRequest);
            //var addedObj = _db.Accounts.Add(obj);

            await _db.SaveChangesAsync();

            return new AccountCreationResponseDTO
            {
                Success = true,
                Message = "Account created successfully",
                AccountId = addedObj.Entity.Id
            };
        }

        public async Task<int> DeleteAccount(string accountId)
        {
            var obj = await _db.Accounts.FirstOrDefaultAsync(u => u.Id == accountId);
            if (obj == null)
            {
                return 0;
            }
            _db.Accounts.Remove(obj);
            await _db.SaveChangesAsync();
            return 1;
        }

        public  List<AccountDTO> GetAllAccounts(string userId)
        {
            List<Account> accounts =  _db.Accounts.Where(u => u.UserId == userId).ToList();
            List<AccountDTO> accountDTOs = _mapper.Map<List<Account>, List<AccountDTO>>(accounts);
            return accountDTOs;
        }
    }
}
