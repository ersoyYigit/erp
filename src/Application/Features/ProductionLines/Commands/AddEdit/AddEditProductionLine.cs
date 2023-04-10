using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Inventory;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Wrapper;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.ProductionLines.Commands.AddEdit
{
    public partial class AddEditProductionLineCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

    }

    internal class AddEditProductionLineCommandHandler : IRequestHandler<AddEditProductionLineCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditProductionLineCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditProductionLineCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditProductionLineCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditProductionLineCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var productionLine = _mapper.Map<ProductionLine>(command);
                await _unitOfWork.Repository<ProductionLine>().AddAsync(productionLine);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllProductionLinesCacheKey);
                return await Result<int>.SuccessAsync(productionLine.Id, _localizer["ProductionLine Saved"]);
            }
            else
            {
                var productionLine = await _unitOfWork.Repository<ProductionLine>().GetByIdAsync(command.Id);
                if (productionLine != null)
                {
                    productionLine.Name = command.Name ?? productionLine.Name;
                    productionLine.Code = command.Code ?? productionLine.Code;
                    productionLine.Description = command.Description ?? productionLine.Description;
                    await _unitOfWork.Repository<ProductionLine>().UpdateAsync(productionLine);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllProductionLinesCacheKey);
                    return await Result<int>.SuccessAsync(productionLine.Id, _localizer["ProductionLine Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["ProductionLine Not Found!"]);
                }
            }
        }
    }
}
