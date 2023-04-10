using AutoMapper;
using ArdaManager.Infrastructure.Models.Audit;
using ArdaManager.Application.Responses.Audit;

namespace ArdaManager.Infrastructure.Mappings
{
    public class AuditProfile : Profile
    {
        public AuditProfile()
        {
            CreateMap<AuditResponse, Audit>().ReverseMap();
        }
    }
}