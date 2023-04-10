using AutoMapper;
using ArdaManager.Infrastructure.Models.Identity;
using ArdaManager.Application.Responses.Identity;

namespace ArdaManager.Infrastructure.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserResponse, VappUser>().ReverseMap();
            CreateMap<ChatUserResponse, VappUser>().ReverseMap()
                .ForMember(dest => dest.EmailAddress, source => source.MapFrom(source => source.Email)); //Specific Mapping
        }
    }
}