using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Corporation;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.CompanyFairs.Commands.Delete
{
    public class DeleteCompanyFairCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteCompanyFairCommandHandler : IRequestHandler<DeleteCompanyFairCommand, Result<int>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IStringLocalizer<DeleteCompanyFairCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteCompanyFairCommandHandler(IUnitOfWork<int> unitOfWork, IProductRepository productRepository, IStringLocalizer<DeleteCompanyFairCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteCompanyFairCommand command, CancellationToken cancellationToken)
        {
            var isCompanyFairUsed = false;// await _productRepository.IsCompanyFairUsed(command.Id);
            if (!isCompanyFairUsed)
            {
                var companyFair = await _unitOfWork.Repository<CompanyFair>().GetByIdAsync(command.Id);
                if (companyFair != null)
                {
                    await _unitOfWork.Repository<CompanyFair>().DeleteAsync(companyFair);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllCompanyFairsCacheKey);
                    return await Result<int>.SuccessAsync(companyFair.Id, _localizer["CompanyFair Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["CompanyFair Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}
