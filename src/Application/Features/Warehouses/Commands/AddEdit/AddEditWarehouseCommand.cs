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

namespace ArdaManager.Application.Features.Warehouses.Commands.AddEdit
{
    public partial class AddEditWarehouseCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }

    }

    internal class AddEditWarehouseCommandHandler : IRequestHandler<AddEditWarehouseCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditWarehouseCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditWarehouseCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditWarehouseCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditWarehouseCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var warehouse = _mapper.Map<Warehouse>(command);
                await _unitOfWork.Repository<Warehouse>().AddAsync(warehouse);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllWarehousesCacheKey);
                return await Result<int>.SuccessAsync(warehouse.Id, _localizer["Warehouse Saved"]);
            }
            else
            {
                var warehouse = await _unitOfWork.Repository<Warehouse>().GetByIdAsync(command.Id);
                if (warehouse != null)
                {
                    warehouse.Name = command.Name ?? warehouse.Name;
                    warehouse.Code = command.Code ?? warehouse.Code;
                    warehouse.Description = command.Description ?? warehouse.Description;
                    await _unitOfWork.Repository<Warehouse>().UpdateAsync(warehouse);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllWarehousesCacheKey);
                    return await Result<int>.SuccessAsync(warehouse.Id, _localizer["Warehouse Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Warehouse Not Found!"]);
                }
            }
        }
    }
}
