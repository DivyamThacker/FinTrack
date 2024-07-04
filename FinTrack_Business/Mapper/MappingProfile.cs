using AutoMapper;
using FinTrack_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using FinTrack_DataAccess;

namespace FinTrack_Business.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Budget, BudgetDTO>().ReverseMap();
            CreateMap<Goal, GoalDTO>().ReverseMap();
            CreateMap<Record, RecordDTO>().ReverseMap();
            CreateMap<Transaction, TransactionDTO>().ReverseMap();
            CreateMap<Account, AccountDTO>().ReverseMap();
            CreateMap<Trade, TradeDTO>().ReverseMap();
            CreateMap<MarketData, MarketDataDTO>().ReverseMap();    
            CreateMap<Holding, HoldingDTO>().ReverseMap();
            //CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
