using ArdaManager.Application.Features.Docs.PurchaseOrders.Commands;
using ArdaManager.Application.Features.Docs.PurchaseOrders.Queries;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Docs.PurchaseOrder
{
    public interface IPurchaseOrderManager : IManager
    {
        Task<IResult<List<PurchaseOrderResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(UpsertPurchaseOrderCommand request);

        Task<IResult<int>> DeleteAsync(int id);
    }
}
