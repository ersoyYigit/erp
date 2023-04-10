using AutoMapper;
using ArdaManager.Application.Requests.Identity;
using ArdaManager.Application.Responses.Identity;

namespace ArdaManager.Client.Infrastructure.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<PermissionResponse, PermissionRequest>().ReverseMap();
            CreateMap<RoleClaimResponse, RoleClaimRequest>().ReverseMap();
        }
    }
}