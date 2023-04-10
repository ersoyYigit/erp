using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Domain.Entities.Report.Warehouse;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Wrapper;
using AutoMapper;
using LazyCache;
using MediatR;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Reports.Warehouses
{
    public class GetAllWarehousesStocksQuery : IRequest<Result<List<WarehouseReport>>>
    {
        public GetAllWarehousesStocksQuery()
        {
        }
        public int WarehouseId { get; set; }
    }

    internal class GetAllWarehousesCachedQueryHandler : IRequestHandler<GetAllWarehousesStocksQuery, Result<List<WarehouseReport>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllWarehousesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<WarehouseReport>>> Handle(GetAllWarehousesStocksQuery request, CancellationToken cancellationToken)
        {

            var parameters = new List<SqlParameter>{
                new SqlParameter("@WarehouseID", SqlDbType.Int) { Value = request.WarehouseId }
            };

            var query = "sp_get_warehouses_stocks";

            var result = await _unitOfWork.VappRepository.ExecuteStoredProcedureAsync<WarehouseReport>(query, parameters.ToArray());

            return Result<List<WarehouseReport>>.Success(result.ToList());
        }

    }
}
