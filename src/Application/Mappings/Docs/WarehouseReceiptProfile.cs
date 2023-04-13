using AutoMapper;
using ArdaManager.Application.Features.Docs.TemplateWorkOrders.Commands.AddEdit;
using ArdaManager.Application.Features.Docs.TemplateWorkOrders.Queries.GetAll;
using ArdaManager.Domain.Entities.Transactions;
using ArdaManager.Application.Features.Docs.TemplateWorkOrders.Queries.GetById;
using ArdaManager.Application.Features.Docs.WarehouseReceipts.Commands.AddEdit;
using ArdaManager.Application.Features.Docs.WarehouseReceipts.Queries.GetById;
using ArdaManager.Application.Features.Docs.WarehouseReceipts.Queries.GetAll;
using ArdaManager.Domain.Entities.Transactions.WarehouseDocs;
using ArdaManager.Application.Features.Docs.PurchaseOffers.Commands;
using ArdaManager.Application.Features.Docs.PurchaseOffers.Queries;
using ArdaManager.Application.Features.Docs.PurchaseRequests.Queries;
using ArdaManager.Domain.Entities.Transactions.Purchase;
using ArdaManager.Application.Features.Docs.PurchaseOrders.Queries;

namespace ArdaManager.Application.Mappings.Docs
{
    public class WarehouseReceiptProfile : Profile
    {
        public WarehouseReceiptProfile()
        {
            CreateMap<AddEditWarehouseReceiptCommand, WarehouseReceipt>().ReverseMap();
            CreateMap<GetAllWarehouseReceiptsResponse, WarehouseReceipt>().ReverseMap();
            CreateMap<GetWarehouseReceiptByIdResponse, WarehouseReceipt>().ReverseMap();


            CreateMap<GetAllWarehouseReceiptsResponse, WarehouseReceipt>().ReverseMap();
            CreateMap<WarehouseReceiptRowsDto, WarehouseReceiptRow>().ReverseMap();

            CreateMap<AddEditWarehouseReceiptCommand, WarehouseReceipt>().ReverseMap();



            CreateMap<GetAllWarehouseReceiptsResponse, AddEditWarehouseReceiptCommand>()
                .ForMember(dest => dest.PrevDocNo, opt => opt.MapFrom(src => src.PrevDoc.DocNo))
                .ForMember(dest => dest.NextDocNo, opt => opt.MapFrom(src => src.NextDoc.DocNo)).ReverseMap();



            /*PREV RELATION*/
            CreateMap<PurchaseOrderRowResponse, WarehouseReceiptRow>().ReverseMap();
            CreateMap<PurchaseOrderResponse, AddEditWarehouseReceiptCommand>()
                .ForMember(dest => dest.PrevDocNo, opt => opt.MapFrom(src => src.DocNo))
                .ForMember(dest => dest.PrevDocId, opt => opt.MapFrom(src => src.Id)).ReverseMap();
        }
    }
}