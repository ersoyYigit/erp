using ArdaManager.Application.Features.Docs.PurchaseRequests.Commands.AddEdit;
using ArdaManager.Application.Features.Docs.PurchaseRequests.Queries;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Docs.PurcheaseRequest
{
    public interface IPurchaseRequestManager : IManager
    {
        Task<IResult<List<PurchaseRequestDto>>> GetAllAsync();


        Task<IResult<int>> SaveAsync(AddEditPurchaseRequestCommand request);

        Task<IResult<int>> DeleteAsync(int id);
    }
}
