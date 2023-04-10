using ArdaManager.Application.Features.Docs.WarehouseReceipts.Commands.AddEdit;
using ArdaManager.Application.Features.Docs.WarehouseReceipts.Queries.GetAll;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Transactions;
using ArdaManager.Domain.Entities.Transactions.WarehouseDocs;
using ArdaManager.Domain.Enums.Doc;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Wrapper;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Docs.WarehouseReceipts.Commands.AddEdit
{
    public partial class AddEditWarehouseReceiptCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public DocType DocType { get; set; }
        public DateTime? DocDate { get; set; }
        public string Description { get; set; }
        public string RequesterId { get; set; }
        public string RequesterName { get; set; }
        public string RequesterDepartment { get; set; }
        public int? PurchaseRequestId { get; set; }
        public int? PrevDocId { get; set; }
        public string PrevDocNo { get; set; }
        public int? NextDocId { get; set; }
        public string NextDocNo { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public DateTime ExchangeDate { get; set; }
        public decimal ExchangeRate { get; set; }

        public WarehouseReceiptType WarehouseReceiptType { get; set; }
        public string WayBillNo { get; set; }
        public DateTime? WayBillDate { get; set; }


        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public int? RelatedWarehouseId { get; set; }
        public string WarehouseOfficerId { get; set; }
        public string WarehouseOfficerName { get; set; }
        public int? RelatedCompanyId { get; set; }
        public string RelatedCompanyName { get; set; }


        public ICollection<WarehouseReceiptRowsDto> WarehouseReceiptRowsDto { get; set; }
        public int? PurchaseOrderId { get;  set; }
        public string PurchaseOrderDocNo { get;  set; }
        public string PurchaseRequestDocNo { get;  set; }
        public DocState DocState { get;  set; }
    }



    internal class AddEditWarehouseReceiptCommandHandler : IRequestHandler<AddEditWarehouseReceiptCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditWarehouseReceiptCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditWarehouseReceiptCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditWarehouseReceiptCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditWarehouseReceiptCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                try
                {

                    //var warehouseReceipt = _mapper.Map<WarehouseReceipt>(command);
                    
                    var warehouseReceipt = _mapper.Map<WarehouseReceipt>(command);
                    warehouseReceipt.WarehouseReceiptRows = _mapper.Map<ICollection<WarehouseReceiptRow>>(command.WarehouseReceiptRowsDto);


                    await _unitOfWork.Repository<WarehouseReceipt>().AddAsync(warehouseReceipt);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllWarehouseReceiptsCacheKey + warehouseReceipt.WarehouseReceiptType.ToString());
                    return await Result<int>.SuccessAsync(warehouseReceipt.Id, _localizer["Depo Fişi Kaydedildi"]);
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

                var warehouseReceipt = _unitOfWork.Repository<WarehouseReceipt>().Entities.Include(x=> x.WarehouseReceiptRows).Where(x=> x.Id == command.Id).ToListAsync().Result.First();
                    if (warehouseReceipt != null)
                    {
                        warehouseReceipt.DocNo = command.DocNo;
                        warehouseReceipt.DocType = command.DocType;
                        warehouseReceipt.DocDate = command.DocDate;
                        warehouseReceipt.Description = command.Description;

                        warehouseReceipt.RequesterDepartment = command.RequesterDepartment;
                        warehouseReceipt.RequesterName = command.RequesterName;
                        warehouseReceipt.RequesterId = command.RequesterId;
                        warehouseReceipt.PurchaseRequestId = command.PurchaseRequestId;
                        warehouseReceipt.NextDocId = command.NextDocId;
                        warehouseReceipt.PrevDocId = command.PrevDocId;
                        warehouseReceipt.PurchaseOrderDocNo = command.PurchaseOrderDocNo;
                        warehouseReceipt.PurchaseRequestDocNo = command.PurchaseRequestDocNo;
                        warehouseReceipt.DocState = command.DocState;
                        warehouseReceipt.PurchaseOrderId = command.PurchaseOrderId;



                        warehouseReceipt.WarehouseReceiptType = command.WarehouseReceiptType;
                        warehouseReceipt.WayBillNo = command.WayBillNo;
                        warehouseReceipt.WayBillDate = command.WayBillDate;

                        warehouseReceipt.WarehouseId = command.WarehouseId;
                        warehouseReceipt.RelatedWarehouseId = command.RelatedWarehouseId;
                        warehouseReceipt.WarehouseOfficerId = command.WarehouseOfficerId;
                        warehouseReceipt.RelatedCompanyId = command.RelatedCompanyId;

                        var modifiedOrNewRows = command.WarehouseReceiptRowsDto
                        .Select(r => new WarehouseReceiptRow
                        {
                            Id = r.Id,
                            ProductId = r.ProductId,
                            Quantity = r.Quantity,
                            Price = r.Price,
                            RackId = (r.RackId == 0 )? null : r.RackId,
                            RowNo = r.RowNo,
                            WarehouseReceiptId = r.WarehouseReceiptId,
                            MeasurementUnitId = r.MeasurementUnitId,
                            CurrencyId = r.CurrencyId,
                            Description = r.Description,
                            DiscountAmount = r.DiscountAmount,
                            DiscountPercentage = r.DiscountPercentage,
                            ExchangeRate = r.ExchangeRate,
                            PurchaseOrderRowId = r.PurchaseOrderRowId,
                            TaxAmount = r.TaxAmount,
                            TotalAmount = r.TotalAmount,
                            TaxId = r.TaxId
                            
                            // Add other properties you want to select here
                        }).ToList();

                        var deletedRows = warehouseReceipt.WarehouseReceiptRows.Except(modifiedOrNewRows);

                        // Remove the deleted rows from the collection
                        warehouseReceipt.WarehouseReceiptRows.ToList().RemoveAll(r => deletedRows.Contains(r, new WarehouseReceiptRowComparer()));
                        warehouseReceipt.WarehouseReceiptRows = modifiedOrNewRows;


                        await _unitOfWork.Repository<WarehouseReceipt>().UpdateAsync(warehouseReceipt);
                        await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllWarehouseReceiptsCacheKey + warehouseReceipt.WarehouseReceiptType.ToString());
                        return await Result<int>.SuccessAsync(warehouseReceipt.Id, _localizer["Depo Fişi güncellendi"]);
                    }
                    else
                    {
                        return await Result<int>.FailAsync(_localizer["Depo Fişi bulunamadı!"]);
                    }
                }
                catch (Exception ex)
                {
                    return await Result<int>.FailAsync(ex.Message);
                    
                }

            }
        }
    }
}
