using ArdaManager.Application.Responses.Approval;
using ArdaManager.Domain.Entities.Approval;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Infrastructure.Mappings
{
    public class ApprovalScenarioProfile : Profile
    {
        public ApprovalScenarioProfile()
        {
            CreateMap<ApprovalScenario, ApprovalScenarioResponse>().ReverseMap();
            CreateMap<ApprovalStep, ApprovalStepResponse>().ReverseMap();
        }
    }
}
