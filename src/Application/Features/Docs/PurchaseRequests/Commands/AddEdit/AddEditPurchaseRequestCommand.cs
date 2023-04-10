using ArdaManager.Application.Features.Docs.PurchaseRequests.Queries;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Transactions.Purchase;
using ArdaManager.Domain.Entities.Transactions.WarehouseDocs;
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
using static ArdaManager.Shared.Constants.Permission.Permissions;

namespace ArdaManager.Application.Features.Docs.PurchaseRequests.Commands.AddEdit
{
    public partial class AddEditPurchaseRequestCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string DocNo { get; set; }
        public DocType DocType { get; set; }
        public DocState DocState { get; set; }
        public DateTime? DocDate { get; set; }
        public string Description { get; set; }


        public string RequesterId { get; set; }
        public string RequesterName { get; set; }
        public string RequesterDepartment { get; set; }

        public ICollection<PurchaseRequestRowDto> PurchaseRequestRowsDto { get; set; }

    }

    internal class AddEditPurchaseRequestCommandHandler : IRequestHandler<AddEditPurchaseRequestCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditPurchaseRequestCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditPurchaseRequestCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditPurchaseRequestCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditPurchaseRequestCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                try
                {
                    var purchaseRequest = _mapper.Map<PurchaseRequest>(command);
                    purchaseRequest.PurchaseRequestRows = _mapper.Map<ICollection<PurchaseRequestRow>>(command.PurchaseRequestRowsDto);
                    //purchaseRequest.PurchaseRequestRows = command.PurchaseRequestRows;

                    await _unitOfWork.Repository<PurchaseRequest>().AddAsync(purchaseRequest);
                    //await _unitOfWork.Commit(cancellationToken);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllPurchaseRequestsCacheKey);
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
                    var existingPurchaseRequest = await _unitOfWork.Repository<PurchaseRequest>().GetByIdAsync(command.Id);

                    if (existingPurchaseRequest == null)
                    {
                        return await Result<int>.FailAsync(_localizer["Satınalma talebi bulunamadı!"]);
                    }

                    _mapper.Map(command, existingPurchaseRequest);
                    existingPurchaseRequest.PurchaseRequestRows.Clear();
                    existingPurchaseRequest.PurchaseRequestRows = _mapper.Map<ICollection<PurchaseRequestRow>>(command.PurchaseRequestRowsDto);
                    //existingPurchaseRequest.PurchaseRequestRows = command.PurchaseRequestRows;

                    await _unitOfWork.Repository<PurchaseRequest>().UpdateAsync(existingPurchaseRequest);
                    //await _unitOfWork.Commit(cancellationToken);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllPurchaseRequestsCacheKey);

                    return await Result<int>.SuccessAsync(existingPurchaseRequest.Id, "Talep güncellendi!");

                }
                catch (Exception ex)
                {
                    return await Result<int>.FailAsync(ex.Message);

                }

            }
        }
    }
}
