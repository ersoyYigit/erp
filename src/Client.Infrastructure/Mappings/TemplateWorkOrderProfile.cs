using AutoMapper;
using ArdaManager.Application.Requests.Identity;
using ArdaManager.Application.Responses.Identity;
using ArdaManager.Application.Features.Docs.TemplateWorkOrders.Queries.GetById;
using ArdaManager.Application.Features.Docs.TemplateWorkOrders.Commands.AddEdit;

namespace ArdaManager.Client.Infrastructure.Mappings
{
    public class TemplateWorkOrderProfile : Profile
    {
        public TemplateWorkOrderProfile()
        {
            CreateMap<GetTemplateWorkOrderByIdResponse, AddEditTemplateWorkOrderCommand>().ReverseMap();
            //CreateMap<RoleClaimResponse, RoleClaimRequest>().ReverseMap();
        }
    }
}