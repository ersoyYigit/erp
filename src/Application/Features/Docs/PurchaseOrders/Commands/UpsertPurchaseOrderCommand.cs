using ArdaManager.Application.Features.Docs.PurchaseOrders.Queries;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Transactions.Purchase;
using ArdaManager.Domain.Enums.Doc;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Wrapper;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Docs.PurchaseOrders.Commands
{
    public partial class UpsertPurchaseOrderCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public DocType DocType { get; set; }
        public DateTime? DocDate { get; set; }
        public string Description { get; set; }

        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string RequesterId { get; set; }
        public string RequesterName { get; set; }
        public string RequesterDepartment { get; set; }

        public int? PurchaseRequestId { get; set; }
        public int? PurchaseOfferId { get; set; }
        public int? PrevDocId { get; set; }
        public string PrevDocNo { get; set; }
        public int? NextDocId { get; set; }
        public string NextDocNo { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public DateTime ExchangeDate { get; set; }
        public decimal ExchangeRate { get; set; }

        public ICollection<PurchaseOrderRowResponse> PurchaseOrderRowResponse { get; set; }
    }

    internal class UpsertPurchaseOrderCommandHandler : IRequestHandler<UpsertPurchaseOrderCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpsertPurchaseOrderCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public UpsertPurchaseOrderCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<UpsertPurchaseOrderCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(UpsertPurchaseOrderCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                try
                {
                    var purchaseOrder = _mapper.Map<PurchaseOrder>(command);
                    purchaseOrder.PurchaseOrderRows = _mapper.Map<ICollection<PurchaseOrderRow>>(command.PurchaseOrderRowResponse);

                    await _unitOfWork.Repository<PurchaseOrder>().AddAsync(purchaseOrder);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllPurchaseOrdersCacheKey);
                    return await Result<int>.SuccessAsync(purchaseOrder.Id, "Sipariş kaydedildi!");
                }
                catch (Exception ex)
                {
                    return await Result<int>.FailAsync(ex.Message);
                }
            }
            else
            {
                try
                {
                    var existingPurchaseOrder = await _unitOfWork.Repository<PurchaseOrder>().GetByIdAsync(command.Id);

                    if (existingPurchaseOrder == null)
                    {
                        return await Result<int>.FailAsync(_localizer["Satınalma siparişi bulunamadı!"]);
                    }

                    _mapper.Map(command, existingPurchaseOrder);
                    existingPurchaseOrder.PurchaseOrderRows.Clear();
                    existingPurchaseOrder.PurchaseOrderRows = _mapper.Map<ICollection<PurchaseOrderRow>>(command.PurchaseOrderRowResponse);

                    await _unitOfWork.Repository<PurchaseOrder>().UpdateAsync(existingPurchaseOrder);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllPurchaseOrdersCacheKey);

                    return await Result<int>.SuccessAsync(existingPurchaseOrder.Id, "Sipariş güncellendi!");
                }
                catch (Exception ex)
                {
                    return await Result<int>.FailAsync(ex.Message);
                }
            }
        }
    }
}
