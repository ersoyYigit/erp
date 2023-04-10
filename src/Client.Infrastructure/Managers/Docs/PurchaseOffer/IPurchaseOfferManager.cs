using ArdaManager.Application.Features.Docs.PurchaseOffers.Commands;
using ArdaManager.Application.Features.Docs.PurchaseOffers.Queries;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Docs.PurchaseOffer
{
    public interface IPurchaseOfferManager : IManager
    {
        Task<IResult<List<PurchaseOfferResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(UpsertPurchaseOfferCommand request);
        Task<IResult<int>> ChooseAsync(ChoosePurchaseOfferCommand request);

        Task<IResult<int>> DeleteAsync(int id);
    }
}
