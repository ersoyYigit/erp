using ArdaManager.Application.Features.Categories.Queries.GetAll;
using ArdaManager.Application.Features.Currencies.Queries;
using ArdaManager.Application.Features.MeasurementUnits.Queries.GetAll;
using ArdaManager.Application.Features.Taxes.Queries;
using ArdaManager.Application.Responses.Identity;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.GlobalVariable
{
    public interface IGlobalVariableService : IManager
    {
        List<UserResponse> AppUsers { get; set; }
        List<GetAllMeasurementUnitsResponse> MeasurementUnits { get; set; }
        List<GetAllCurrenciesResponse> Currencies { get; set; }
        List<GetAllTaxesResponse> Taxes { get; set; }
        //List<GetAllCategoriesResponse> Categories { get; set; }
        Task<IResult<List<UserResponse>>> GetAllUsersAsync();

    }
}
