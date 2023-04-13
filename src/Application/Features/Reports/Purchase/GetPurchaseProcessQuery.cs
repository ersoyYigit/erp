using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Report.Purchase;
using ArdaManager.Domain.Entities.Report.Warehouse;
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

namespace ArdaManager.Application.Features.Reports.Purchase
{
    public class GetPurchaseProcessQuery : IRequest<Result<List<GetPurchaseProcessResponse>>>
    {
        public GetPurchaseProcessQuery()
        {
        }
    }

    internal class GetPurchaseProcessQueryHandler : IRequestHandler<GetPurchaseProcessQuery, Result<List<GetPurchaseProcessResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetPurchaseProcessQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<GetPurchaseProcessResponse>>> Handle(GetPurchaseProcessQuery request, CancellationToken cancellationToken)
        {


            var query = "vw_PurchaseProcess";

            var result = await _unitOfWork.VappRepository.ExecuteViewAsync<GetPurchaseProcessResponse>(query);


            var query2 = "vw_PurchaseProcessDetail";

            var result2 = await _unitOfWork.VappRepository.ExecuteViewAsync<PurchaseProcessRowResult>(query2);




            return Result<List<GetPurchaseProcessResponse>>.Success(result.ToList());
        }

    }
}
