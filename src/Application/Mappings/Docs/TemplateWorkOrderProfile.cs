using AutoMapper;
using ArdaManager.Application.Features.Docs.TemplateWorkOrders.Commands.AddEdit;
using ArdaManager.Application.Features.Docs.TemplateWorkOrders.Queries.GetAll;
using ArdaManager.Domain.Entities.Transactions;
using ArdaManager.Application.Features.Docs.TemplateWorkOrders.Queries.GetById;

namespace ArdaManager.Application.Mappings.Docs
{
    public class TemplateWorkOrderProfile : Profile
    {
        public TemplateWorkOrderProfile()
        {
            CreateMap<AddEditTemplateWorkOrderCommand, TemplateWorkOrder>().ReverseMap();
            CreateMap<GetTemplateWorkOrderByIdResponse, TemplateWorkOrder>().ReverseMap();
            CreateMap<GetAllTemplateWorkOrdersResponse, TemplateWorkOrder>().ReverseMap();
        }
    }
}