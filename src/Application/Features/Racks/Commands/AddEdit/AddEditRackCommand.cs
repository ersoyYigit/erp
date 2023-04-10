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

namespace ArdaManager.Application.Features.Racks.Commands.AddEdit
{
    public partial class AddEditRackCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }
        public string Section { get; set; }
        public string SectionCode { get; set; }


        public int WarehouseId { get; set; }

    }

    internal class AddEditRackCommandHandler : IRequestHandler<AddEditRackCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditRackCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditRackCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditRackCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditRackCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var rack = _mapper.Map<Rack>(command);
                await _unitOfWork.Repository<Rack>().AddAsync(rack);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllRacksCacheKey);
                return await Result<int>.SuccessAsync(rack.Id, _localizer["Rack Saved"]);
            }
            else
            {
                var rack = await _unitOfWork.Repository<Rack>().GetByIdAsync(command.Id);
                if (rack != null)
                {
                    rack.Code = command.Code ?? rack.Code;
                    rack.Name = command.Name ?? rack.Name;
                    rack.Section = command.Section ?? rack.Section;
                    rack.SectionCode = command.SectionCode ?? rack.SectionCode;
                    rack.Description = command.Description ?? rack.Description;
                    rack.WarehouseId = (command.WarehouseId == 0 ) ? rack.WarehouseId : command.WarehouseId;
                    await _unitOfWork.Repository<Rack>().UpdateAsync(rack);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllRacksCacheKey);
                    return await Result<int>.SuccessAsync(rack.Id, _localizer["Rack Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Rack Not Found!"]);
                }
            }
        }
    }
}
