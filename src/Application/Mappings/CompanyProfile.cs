using ArdaManager.Application.Features.Companies.Commands.AddEdit;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Domain.Entities.Corporation;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Mappings
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<AddEditCompanyCommand, Company>().ReverseMap();
        }
    }
}
