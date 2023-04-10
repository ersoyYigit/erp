using ArdaManager.Application.Features.Docs;
using ArdaManager.Application.Features.Docs.PurchaseOffers.Queries;
using ArdaManager.Domain.Entities;
using ArdaManager.Domain.Entities.Transactions.Purchase;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Mappings.Docs
{
    public class BaseDocProfile : Profile
    {
        public BaseDocProfile()
        {
            CreateMap<BaseDocResponse, BaseDoc>().ReverseMap();
        }
    }
}
