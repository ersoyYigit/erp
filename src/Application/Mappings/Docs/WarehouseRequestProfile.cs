using ArdaManager.Application.Features.Docs.WarehouseRequests.Commands;
using ArdaManager.Application.Features.Docs.WarehouseRequests.Queries;
using ArdaManager.Application.Features.Docs.PurchaseRequests.Queries;
using ArdaManager.Domain.Entities.Transactions.Purchase;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArdaManager.Domain.Entities.Transactions.WarehouseDocs;

namespace ArdaManager.Application.Mappings.Docs
{
    public class WarehouseRequestProfile : Profile
    {
        public WarehouseRequestProfile()
        {
            CreateMap<WarehouseRequestResponse, WarehouseRequest>().ReverseMap();
            CreateMap<WarehouseRequestRowResponse, WarehouseRequestRow>().ReverseMap();
            CreateMap<UpsertWarehouseRequestCommand, WarehouseRequest>().ReverseMap();



            CreateMap<WarehouseRequestResponse, UpsertWarehouseRequestCommand>()
                .ForMember(dest => dest.PrevDocNo, opt => opt.MapFrom(src => src.PrevDoc.DocNo))
                .ForMember(dest => dest.NextDocNo, opt => opt.MapFrom(src => src.NextDoc.DocNo));



            /*PREV RELATION*/
            CreateMap<PurchaseRequestRowDto, WarehouseRequestRow>().ReverseMap();
            CreateMap<PurchaseRequestDto, UpsertWarehouseRequestCommand>()
                .ForMember(dest => dest.PrevDocNo, opt => opt.MapFrom(src => src.DocNo))
                .ForMember(dest => dest.PrevDocId, opt => opt.MapFrom(src => src.Id)).ReverseMap();





        }

    }
}
