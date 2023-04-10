using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Human;
using ArdaManager.Domain.Entities.Transactions.Purchase;
using ArdaManager.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Docs.PurchaseRequests.Queries.GetById
{
    public class GetPurchaseRequestByIdQuery : IRequest<Result<PurchaseRequestDto>>
    {
        public int Id { get; set; }
    }

    public class GetPurchaseRequestByIdQueryHandler : IRequestHandler<GetPurchaseRequestByIdQuery, Result<PurchaseRequestDto>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetPurchaseRequestByIdQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PurchaseRequestDto>> Handle(GetPurchaseRequestByIdQuery query, CancellationToken cancellationToken)
        {
            var purchaseRequest = await _unitOfWork.Repository<PurchaseRequest>().GetByIdAsync(query.Id);

            if (purchaseRequest == null)
            {
                return await Result<PurchaseRequestDto>.FailAsync("Talep bulunamadı");
            }

            var purchaseRequestRows = await _unitOfWork.Repository<PurchaseRequestRow>()
                .GetAllAsync(
                    row => row.PurchaseRequestId == query.Id,
                    include: r => r.Include(row => row.MeasurementUnit)
                                  .Include(row => row.Product)
                );

            

            var purchaseRequestDto = new PurchaseRequestDto
            {
                DocNo = purchaseRequest.DocNo,
                DocDate = purchaseRequest.DocDate,
                DocType = purchaseRequest.DocType,
                DocState = purchaseRequest.DocState,
                Id = purchaseRequest.Id,
                RequesterId = purchaseRequest.RequesterId,
                RequirementDate = purchaseRequest.RequirementDate,
                Description = purchaseRequest.Description,
                RequesterName = purchaseRequest.RequesterName,
                RequesterUserId = purchaseRequest.RequesterId,
                RequesterDepartment =purchaseRequest.RequesterDepartment,
                CreatedBy = purchaseRequest.CreatedBy,
                //PurchaseRequestRows = e.PurchaseRequestRows,
                PurchaseRequestRowsDto = purchaseRequest.PurchaseRequestRows
                    .Select(r => new PurchaseRequestRowDto
                    {
                        Id = r.Id,
                        ProductId = r.ProductId,
                        ProductName = r.Product.Name,
                        Quantity = r.Quantity,
                        MeasurementUnitCode = r.MeasurementUnit.Code,
                        MeasurementUnitId = r.MeasurementUnit.Id,
                        MeasurementUnitName = r.MeasurementUnit.Name,
                        ProductCode = r.Product.Code,
                        RowNo = r.RowNo,
                        PurchaseRequestId = r.PurchaseRequestId,
                        Description = r.Description
                        // Add other properties you want to select here
                    }).ToList()
            };

            return await Result<PurchaseRequestDto>.SuccessAsync(purchaseRequestDto);
        }
    }
    
}