using ArdaManager.Application.Features.Countries.Queries.GetById;
using ArdaManager.Application.Features.Currencies.Commands;
using ArdaManager.Application.Features.Currencies.Queries;
using ArdaManager.Domain.Entities.Addressing;
using ArdaManager.Domain.Entities.Misc;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Mappings
{
    public class CurrencyProfile : Profile
    {
        public CurrencyProfile()
        {
            CreateMap<GetAllCurrenciesResponse, Currency>().ReverseMap();
            CreateMap<UpsertCurrencyCommand, Currency>().ReverseMap();
        }
    }

    public class ExchangeRateProfile : Profile
    {
        public ExchangeRateProfile()
        {
            CreateMap<UpsertExchangeRatesCommand, ExchangeRate>().ReverseMap();
            
            CreateMap<ExchangeRate, GetEffectiveSellRateResponse>()
                .ForMember(dest => dest.EffectiveSellRate, opt => opt.MapFrom(src => src.BanknoteSelling)).ReverseMap()
                .ForMember(dest => dest.RateDate, opt => opt.MapFrom(src => src.RateDate)).ReverseMap();

            CreateMap<ExchangeRate, GetExchangeRatesByDateResponse>()
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.Currency.Code))
                .ForMember(dest => dest.CurrencyName, opt => opt.MapFrom(src => src.Currency.Name))
                .ForMember(dest => dest.ForexBuying, opt => opt.MapFrom(src => src.ForexBuying))
                .ForMember(dest => dest.ForexSelling, opt => opt.MapFrom(src => src.ForexSelling))
                .ForMember(dest => dest.BanknoteBuying, opt => opt.MapFrom(src => src.BanknoteBuying))
                .ForMember(dest => dest.BanknoteSelling, opt => opt.MapFrom(src => src.BanknoteSelling));

        }
    }


}
