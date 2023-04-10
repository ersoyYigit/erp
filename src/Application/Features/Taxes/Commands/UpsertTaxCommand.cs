using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Misc;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Wrapper;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Taxes.Commands
{
    public class UpsertTaxCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int Percent { get; set; }
    }

    public class UpsertTaxCommandHandler : IRequestHandler<UpsertTaxCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;

        public UpsertTaxCommandHandler(IMapper mapper, IUnitOfWork<int> unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(UpsertTaxCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var currency = _mapper.Map<Tax>(command);
                await _unitOfWork.Repository<Tax>().AddAsync(currency);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllTaxesCacheKey);
                return await Result<int>.SuccessAsync(currency.Id, "Vergi dilimi kaydedildi");
            }
            else
            {
                var currency = await _unitOfWork.Repository<Tax>().GetByIdAsync(command.Id);
                if (currency == null)
                {
                    return await Result<int>.FailAsync("Vergi dilimi bulunamadı.");
                }
                currency.Name = command.Name;
                currency.Code = command.Code;
                currency.Description = command.Description;
                currency.Percent = command.Percent;
                await _unitOfWork.Repository<Tax>().UpdateAsync(currency);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllTaxesCacheKey);
                return await Result<int>.SuccessAsync(currency.Id, "Vergi dilimi güncellendi");
            }
        }
    }
}
