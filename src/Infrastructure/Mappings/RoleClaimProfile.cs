using AutoMapper;
using ArdaManager.Application.Requests.Identity;
using ArdaManager.Application.Responses.Identity;
using ArdaManager.Infrastructure.Models.Identity;

namespace ArdaManager.Infrastructure.Mappings
{
    public class RoleClaimProfile : Profile
    {
        public RoleClaimProfile()
        {
            CreateMap<RoleClaimResponse, VappRoleClaim>()
                .ForMember(nameof(VappRoleClaim.ClaimType), opt => opt.MapFrom(c => c.Type))
                .ForMember(nameof(VappRoleClaim.ClaimValue), opt => opt.MapFrom(c => c.Value))
                .ReverseMap();

            CreateMap<RoleClaimRequest, VappRoleClaim>()
                .ForMember(nameof(VappRoleClaim.ClaimType), opt => opt.MapFrom(c => c.Type))
                .ForMember(nameof(VappRoleClaim.ClaimValue), opt => opt.MapFrom(c => c.Value))
                .ReverseMap();
        }
    }
}