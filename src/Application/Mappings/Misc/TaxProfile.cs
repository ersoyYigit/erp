using ArdaManager.Application.Features.Categories.Commands.AddEdit;
using ArdaManager.Application.Features.Taxes.Commands;
using ArdaManager.Application.Features.Taxes.Queries;
using ArdaManager.Domain.Entities.Misc;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Mappings.Misc
{
    public class TaxProfile : Profile 
    {
        public TaxProfile()
        {
            CreateMap<UpsertTaxCommand, Tax>().ReverseMap();
            CreateMap<GetAllTaxesResponse, Tax>().ReverseMap();
        }
    }
}
