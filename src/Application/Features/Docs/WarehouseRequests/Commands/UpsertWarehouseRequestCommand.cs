using ArdaManager.Application.Features.Docs.WarehouseReceipts.Queries.GetAll;
using ArdaManager.Application.Features.Docs.WarehouseRequests.Queries;
using ArdaManager.Application.Interfaces.Repositories;
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
using ArdaManager.Domain.Entities.Transactions.WarehouseDocs;

namespace ArdaManager.Application.Features.Docs.WarehouseRequests.Commands
{
    public partial class UpsertWarehouseRequestCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public DocType DocType { get; set; }
        public DateTime? DocDate { get; set; }
        public DocState DocState { get; set; }
        public string Description { get; set; }
        public string RequesterId { get; set; }
        public string RequesterName { get; set; }
        public string RequesterDepartment { get; set; }
        public int? PrevDocId { get; set; }
        public string PrevDocNo { get; set; }
        public int? NextDocId { get; set; }
        public string NextDocNo { get; set; }
        
        public WarehouseReceiptType WarehouseReceiptType { get; set; }
        

        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public int RelatedWarehouseId { get; set; }
        public string RelatedWarehouseName { get; set; }
        public string WarehouseOfficerId { get; set; }
        public string WarehouseOfficerName { get; set; }
        

        public ICollection<WarehouseRequestRowResponse> WarehouseRequestRowsResponse { get; set; }
    }

    internal class AddEditWarehouseRequestCommandHandler : IRequestHandler<UpsertWarehouseRequestCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditWarehouseRequestCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditWarehouseRequestCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditWarehouseRequestCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(UpsertWarehouseRequestCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                try
                {
                    var purchaseRequest = _mapper.Map<WarehouseRequest>(command);
                    purchaseRequest.WarehouseRequestRows = _mapper.Map<ICollection<WarehouseRequestRow>>(command.WarehouseRequestRowsResponse);
                    //purchaseRequest.WarehouseRequestRows = command.WarehouseRequestRows;

                    await _unitOfWork.Repository<WarehouseRequest>().AddAsync(purchaseRequest);
                    //await _unitOfWork.Commit(cancellationToken);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllWarehouseRequestsCacheKey);
                    return await Result<int>.SuccessAsync(purchaseRequest.Id, "Talep kaydedildi!");

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
                    var existingWarehouseRequest = await _unitOfWork.Repository<WarehouseRequest>().GetByIdAsync(command.Id);

                    if (existingWarehouseRequest == null)
                    {
                        return await Result<int>.FailAsync(_localizer["Depo talebi bulunamadı!"]);
                    }

                    _mapper.Map(command, existingWarehouseRequest);
                    existingWarehouseRequest.WarehouseRequestRows.Clear();
                    existingWarehouseRequest.WarehouseRequestRows = _mapper.Map<ICollection<WarehouseRequestRow>>(command.WarehouseRequestRowsResponse);
                    //existingWarehouseRequest.WarehouseRequestRows = command.WarehouseRequestRows;

                    await _unitOfWork.Repository<WarehouseRequest>().UpdateAsync(existingWarehouseRequest);
                    //await _unitOfWork.Commit(cancellationToken);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllWarehouseRequestsCacheKey);

                    return await Result<int>.SuccessAsync(existingWarehouseRequest.Id, "Talep güncellendi!");

                }
                catch (Exception ex)
                {
                    return await Result<int>.FailAsync(ex.Message);

                }

            }
        }
    }
}
