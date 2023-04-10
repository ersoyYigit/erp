using ArdaManager.Application.Features.Racks.Commands.AddEdit;
using ArdaManager.Application.Features.Racks.Queries.GetAll;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Inventory.Rack
{
    public interface IRackManager : IManager
    {
        Task<IResult<List<GetAllRacksResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditRackCommand request);

        Task<IResult<int>> DeleteAsync(int id);


    }
}
