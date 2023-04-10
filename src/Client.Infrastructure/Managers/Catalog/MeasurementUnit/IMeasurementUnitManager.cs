using ArdaManager.Application.Features.Patterns.Queries.GetAll;
using ArdaManager.Application.Features.MeasurementUnits.Queries.GetAll;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Catalog.MeasurementUnit
{
    public  interface IMeasurementUnitManager : IManager
    {
        Task<IResult<List<GetAllMeasurementUnitsResponse>>> GetAllAsync();
    }
}
