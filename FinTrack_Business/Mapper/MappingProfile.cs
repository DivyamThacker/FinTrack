using AutoMapper;
using FinTrack_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using FinTrack_DataAccess;
using FinTrack;

namespace FinTrack_Business.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Budget, BudgetDTO>().ReverseMap();
        }
    }
}
