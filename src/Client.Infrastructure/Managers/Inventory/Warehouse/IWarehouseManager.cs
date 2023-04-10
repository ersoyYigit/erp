using ArdaManager.Application.Features.Warehouses.Commands.AddEdit;
using ArdaManager.Application.Features.Warehouses.Queries.GetAll;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Inventory.Warehouse
{
    public interface IWarehouseManager : IManager
    {
        Task<IResult<List<GetAllWarehousesResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditWarehouseCommand request);

        Task<IResult<int>> DeleteAsync(int id);


    }
}
