using ArdaManager.Application.Features.Docs.WarehouseReceipts.Commands.AddEdit;
using ArdaManager.Application.Features.Docs.WarehouseReceipts.Queries.GetAll;
using ArdaManager.Application.Features.Docs.WarehouseReceipts.Queries.GetById;
using ArdaManager.Domain.Enums.Doc;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Docs.WarehouseReceipt
{
    public interface IWarehouseReceiptManager : IManager
    {
        Task<IResult<List<GetAllWarehouseReceiptsResponse>>> GetAllAsync();

        Task<IResult<List<GetAllWarehouseReceiptsResponse>>> GetAllByType(WarehouseReceiptType type);
        Task<IResult<List<GetAllWarehouseReceiptsResponse>>> GetAllByWarehouseId(WarehouseReceiptType type, int id);

        Task<IResult<GetWarehouseReceiptByIdResponse>> GetByIdAsync(GetWarehouseReceiptByIdQuery query);

        Task<IResult<int>> SaveAsync(AddEditWarehouseReceiptCommand request);

        Task<IResult<int>> DeleteAsync(int id);




    }
}
