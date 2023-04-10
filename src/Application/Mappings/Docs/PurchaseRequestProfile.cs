using ArdaManager.Application.Features.Docs.PurchaseRequests.Commands.AddEdit;
using ArdaManager.Application.Features.Docs.PurchaseRequests.Queries;
using ArdaManager.Application.Features.Docs.TemplateWorkOrders.Commands.AddEdit;
using ArdaManager.Application.Features.Docs.TemplateWorkOrders.Queries.GetAll;
using ArdaManager.Application.Features.Docs.TemplateWorkOrders.Queries.GetById;
using ArdaManager.Domain.Entities.Transactions;
using ArdaManager.Domain.Entities.Transactions.Purchase;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Mappings.Docs
{
    public class PurchaseRequestProfile : Profile
    {
        public PurchaseRequestProfile()
        {
            CreateMap<PurchaseRequestRowDto, PurchaseRequestRow>().ReverseMap();
            CreateMap<PurchaseRequestDto, PurchaseRequest>().ReverseMap();
            CreateMap<AddEditPurchaseRequestCommand, PurchaseRequest>().ReverseMap();
        }
    }
}
