using AutoMapper;
using ArdaManager.Application.Interfaces.Chat;
using ArdaManager.Application.Models.Chat;
using ArdaManager.Infrastructure.Models.Identity;

namespace ArdaManager.Infrastructure.Mappings
{
    public class ChatHistoryProfile : Profile
    {
        public ChatHistoryProfile()
        {
            CreateMap<ChatHistory<IChatUser>, ChatHistory<VappUser>>().ReverseMap();
        }
    }
}