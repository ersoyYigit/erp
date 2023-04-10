using ArdaManager.Application.Features.ProductionLines.Commands.AddEdit;
using ArdaManager.Application.Features.ProductionLines.Queries.GetAll;
using ArdaManager.Application.Features.ProductionLines.Queries.GetById;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Inventory.ProductionLine
{
    public interface IProductionLineManager : IManager
    {
        Task<IResult<List<GetAllProductionLinesResponse>>> GetAllAsync();

        Task<IResult<GetProductionLineByIdResponse>> GetByIdAsync(GetProductionLineByIdQuery query);

        Task<IResult<int>> SaveAsync(AddEditProductionLineCommand request);

        Task<IResult<int>> DeleteAsync(int id);




    }
}
