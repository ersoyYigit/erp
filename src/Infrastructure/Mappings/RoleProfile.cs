using AutoMapper;
using ArdaManager.Infrastructure.Models.Identity;
using ArdaManager.Application.Responses.Identity;

namespace ArdaManager.Infrastructure.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleResponse, VappRole>().ReverseMap();
        }
    }
}