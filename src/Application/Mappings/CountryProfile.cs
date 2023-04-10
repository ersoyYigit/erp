using ArdaManager.Application.Features.Countries.Queries.GetAll;
using ArdaManager.Application.Features.Countries.Queries.GetById;
using ArdaManager.Domain.Entities.Addressing;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Mappings
{
    
    public class CountryProfile : Profile
    {
        public CountryProfile()
        {
            CreateMap<GetCountryByIdResponse, Country>().ReverseMap();
            CreateMap<GetAllCountriesResponse, Country>().ReverseMap();
        }
    }
}
