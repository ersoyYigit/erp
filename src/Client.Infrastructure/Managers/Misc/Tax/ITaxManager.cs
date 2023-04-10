using ArdaManager.Application.Features.Patterns.Commands.AddEdit;
using ArdaManager.Application.Features.Patterns.Queries.GetAll;
using ArdaManager.Application.Features.Taxes.Commands;
using ArdaManager.Application.Features.Taxes.Queries;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Misc.Tax
{
    public interface ITaxManager : IManager
    {
        Task<IResult<List<GetAllTaxesResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(UpsertTaxCommand request);

        Task<IResult<int>> DeleteAsync(int id);

    }
}
