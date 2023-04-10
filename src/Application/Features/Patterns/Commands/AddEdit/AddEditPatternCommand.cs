using System.ComponentModel.DataAnnotations;
using AutoMapper;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using ArdaManager.Shared.Constants.Application;

namespace ArdaManager.Application.Features.Patterns.Commands.AddEdit
{
    public partial class AddEditPatternCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Tax { get; set; }
    }

    internal class AddEditPatternCommandHandler : IRequestHandler<AddEditPatternCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditPatternCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditPatternCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditPatternCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditPatternCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var pattern = _mapper.Map<Pattern>(command);
                await _unitOfWork.Repository<Pattern>().AddAsync(pattern);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllPatternsCacheKey);
                return await Result<int>.SuccessAsync(pattern.Id, _localizer["Pattern Saved"]);
            }
            else
            {
                var pattern = await _unitOfWork.Repository<Pattern>().GetByIdAsync(command.Id);
                if (pattern != null)
                {
                    pattern.Name = command.Name ?? pattern.Name;
                    pattern.Description = command.Description ?? pattern.Description;
                    await _unitOfWork.Repository<Pattern>().UpdateAsync(pattern);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllPatternsCacheKey);
                    return await Result<int>.SuccessAsync(pattern.Id, _localizer["Pattern Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Pattern Not Found!"]);
                }
            }
        }
    }
}