using ArdaManager.Application.Features.Fairs.Commands.AddEdit;
using ArdaManager.Application.Features.Fairs.Queries.GetAll;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Corporation.Fair
{
    public interface IFairManager : IManager
    {
        Task<IResult<List<GetAllFairsResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditFairCommand request);

        Task<IResult<int>> DeleteAsync(int id);
    }
}
