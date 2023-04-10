using ArdaManager.Application.Features.Cities.Queries.GetAll;
using ArdaManager.Application.Features.Cities.Queries.GetById;
using ArdaManager.Application.Features.Cities.Queries.GetCitiesWithCountryId;
using ArdaManager.Domain.Entities.Addressing;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Mappings
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<GetCityByIdResponse, City>().ReverseMap();//.ForMember( m => m.CountryName , conf => conf.MapFrom(ol => ol.Country.Name));
            CreateMap<GetAllCitiesResponse, City>().ReverseMap();
            CreateMap<GetCitiesByCountryIdResponse, City>().ReverseMap();//.ForMember( m => m.CountryName , conf => conf.MapFrom(ol => ol.Country.Name));


            CreateMap<GetCityByIdResponse, Country>().ReverseMap();

        }
    }
}
