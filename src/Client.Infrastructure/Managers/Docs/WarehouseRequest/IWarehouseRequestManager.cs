using ArdaManager.Application.Features.Docs.WarehouseReceipts.Commands.AddEdit;
using ArdaManager.Application.Features.Docs.WarehouseReceipts.Queries.GetAll;
using ArdaManager.Application.Features.Docs.WarehouseReceipts.Queries.GetById;
using ArdaManager.Application.Features.Docs.WarehouseRequests.Commands;
using ArdaManager.Application.Features.Docs.WarehouseRequests.Queries;
using ArdaManager.Domain.Enums.Doc;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Docs.WarehouseRequest
{
    public interface IWarehouseRequestManager : IManager
    {
        Task<IResult<List<WarehouseRequestResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(UpsertWarehouseRequestCommand request);

        Task<IResult<int>> DeleteAsync(int id);
    }
}
