using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Corporation;
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

namespace ArdaManager.Application.Features.Fairs.Commands.AddEdit
{
    public partial class AddEditFairCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime? Date { get; set; }



    }

    internal class AddEditFairCommandHandler : IRequestHandler<AddEditFairCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditFairCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditFairCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditFairCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditFairCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var fair = _mapper.Map<Fair>(command);
                await _unitOfWork.Repository<Fair>().AddAsync(fair);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllFairsCacheKey);
                return await Result<int>.SuccessAsync(fair.Id, _localizer["Fair Saved"]);
            }
            else
            {
                var fair = await _unitOfWork.Repository<Fair>().GetByIdAsync(command.Id);
                if (fair != null)
                {
                    fair.Code = command.Code ?? fair.Code;
                    fair.Name = command.Name ?? fair.Name;
                    fair.Description = command.Description ?? fair.Description;
                    fair.Type = command.Type ?? fair.Type;
                    fair.Date = command.Date ?? fair.Date;
                    await _unitOfWork.Repository<Fair>().UpdateAsync(fair);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllFairsCacheKey);
                    return await Result<int>.SuccessAsync(fair.Id, _localizer["Fair Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Fair Not Found!"]);
                }
            }
        }
    }
}
