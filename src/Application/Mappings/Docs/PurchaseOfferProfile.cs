﻿using ArdaManager.Application.Features.Docs.PurchaseOffers.Commands;
using ArdaManager.Application.Features.Docs.PurchaseOffers.Queries;
using ArdaManager.Application.Features.Docs.PurchaseRequests.Commands.AddEdit;
using ArdaManager.Application.Features.Docs.PurchaseRequests.Queries;
using ArdaManager.Domain.Entities.Transactions.Purchase;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Mappings.Docs
{
    public class PurchaseOfferProfile : Profile
    {
        public PurchaseOfferProfile()
        {
            CreateMap<PurchaseOfferResponse, PurchaseOffer>().ReverseMap();
            CreateMap<PurchaseOfferRowResponse, PurchaseOfferRow>()
                .ForMember(dest => dest.Currency, opt => opt.Ignore())
                .ForMember(dest => dest.Tax, opt => opt.Ignore()).ReverseMap();
            CreateMap<UpsertPurchaseOfferCommand, PurchaseOffer>().ReverseMap();



            CreateMap<PurchaseOfferResponse, UpsertPurchaseOfferCommand>()
                .ForMember(dest => dest.PrevDocNo, opt => opt.MapFrom(src => src.PrevDoc.DocNo))
                .ForMember(dest => dest.NextDocNo, opt => opt.MapFrom(src => src.NextDoc.DocNo));



            /*PREV RELATION*/
            CreateMap<PurchaseRequestRowDto, PurchaseOfferRow>().ReverseMap();
            CreateMap<PurchaseRequestDto, UpsertPurchaseOfferCommand>()
                .ForMember(dest => dest.PrevDocNo, opt => opt.MapFrom(src => src.DocNo))
                .ForMember(dest => dest.PrevDocId, opt => opt.MapFrom(src => src.Id)).ReverseMap();





        }
    }
}
