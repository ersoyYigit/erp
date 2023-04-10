using ArdaManager.Application.Features.CompanyFairs.Commands.AddEdit;
using ArdaManager.Application.Features.CompanyFairs.Queries.GetAll;
using ArdaManager.Application.Features.CompanyFairs.Queries.GetByCompany;
using ArdaManager.Application.Features.CompanyFairs.Queries.GetById;
using ArdaManager.Domain.Entities.Addressing;
using ArdaManager.Domain.Entities.Corporation;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Mappings
{
    public class CompanyFairsProfile : Profile
    {
        public CompanyFairsProfile()
        {
            CreateMap<AddEditCompanyFairCommand, CompanyFair>().ReverseMap();
            CreateMap<GetCompanyFairByIdResponse, CompanyFair>().ReverseMap();
            CreateMap<GetAllCompanyFairsResponse, CompanyFair>().ReverseMap();
            CreateMap<GetCompanyFairsByCompanyIdResponse, CompanyFair>().ReverseMap();

        }
    }
}
