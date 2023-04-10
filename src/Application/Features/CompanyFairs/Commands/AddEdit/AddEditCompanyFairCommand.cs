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

namespace ArdaManager.Application.Features.CompanyFairs.Commands.AddEdit
{
    
    public partial class AddEditCompanyFairCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public int FairId { get; set; }

    }

    internal class AddEditCompanyFairCommandHandler : IRequestHandler<AddEditCompanyFairCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditCompanyFairCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditCompanyFairCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditCompanyFairCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditCompanyFairCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var companyFair = _mapper.Map<CompanyFair>(command);

                bool any = _unitOfWork.Repository<CompanyFair>().Entities.Where(x => x.CompanyId == companyFair.CompanyId && x.FairId == companyFair.FairId).Any();

                if (!any)
                { 
                    await _unitOfWork.Repository<CompanyFair>().AddAsync(companyFair);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllCompanyFairsCacheKey);
                    return await Result<int>.SuccessAsync(companyFair.Id, _localizer["Fair addec successfully"]);
                }
                else
                    return await Result<int>.FailAsync(_localizer["Fair already exist in context!"]);
            }
            else
            {
                var companyFair = await _unitOfWork.Repository<CompanyFair>().GetByIdAsync(command.Id);
                if (companyFair != null)
                {
                    
                    companyFair.CompanyId = (command.CompanyId == 0) ? companyFair.CompanyId : command.CompanyId;
                    companyFair.FairId = (command.FairId == 0) ? companyFair.FairId : command.FairId;
                    await _unitOfWork.Repository<CompanyFair>().UpdateAsync(companyFair);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllCompanyFairsCacheKey);
                    return await Result<int>.SuccessAsync(companyFair.Id, _localizer["CompanyFair Relation Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["CompanyFair Relation Not Found!"]);
                }
            }
        }
    }
}
