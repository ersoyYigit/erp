using ArdaManager.Application.Features.Countries.Queries.GetAll;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Addressing.Country
{
    public interface ICountryManager : IManager
    {
        Task<IResult<List<GetAllCountriesResponse>>> GetAllAsync();
    }
}
