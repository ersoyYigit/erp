
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Application.Interfaces.Services;
using ArdaManager.Application.Requests;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ArdaManager.Application.Features.Patterns.Commands.AddEdit;
using FluentValidation;

namespace ArdaManager.Application.Features.Patterns.Commands
{
    public partial class ImportPatternsCommand : IRequest<Result<int>>
    {
        public UploadRequest UploadRequest { get; set; }
    }

    internal class ImportPatternsCommandHandler : IRequestHandler<ImportPatternsCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IExcelService _excelService;
        private readonly IMapper _mapper;
        private readonly IValidator<AddEditPatternCommand> _addPatternValidator;
        private readonly IStringLocalizer<ImportPatternsCommandHandler> _localizer;

        public ImportPatternsCommandHandler(
            IUnitOfWork<int> unitOfWork,
            IExcelService excelService,
            IMapper mapper,
            IValidator<AddEditPatternCommand> addPatternValidator,
            IStringLocalizer<ImportPatternsCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _excelService = excelService;
            _mapper = mapper;
            _addPatternValidator = addPatternValidator;
            _localizer = localizer;
        }



        
        public async Task<Result<int>> Handle(ImportPatternsCommand request, CancellationToken cancellationToken)
        {
            var stream = new MemoryStream(request.UploadRequest.Data);
            var result = (await _excelService.ImportAsync(stream, mappers: new Dictionary<string, Func<DataRow, Pattern, object>>
            {
                { "Name", (row,item) => item.Name = row["Name"].ToString() },
                { "Description", (row,item) => item.Description = row["Description"].ToString() }
            }, _localizer["Patterns"]));

            if (result.Succeeded)
            {
                var importedPatterns = result.Data;
                var errors = new List<string>();
                var errorsOccurred = false;
                foreach (var pattern in importedPatterns)
                {
                    var validationResult = await _addPatternValidator.ValidateAsync(_mapper.Map<AddEditPatternCommand>(pattern), cancellationToken);
                    if (validationResult.IsValid)
                    {
                        await _unitOfWork.Repository<Pattern>().AddAsync(pattern);
                    }
                    else
                    {
                        errorsOccurred = true;
                        errors.AddRange(validationResult.Errors.Select(e => $"{(!string.IsNullOrWhiteSpace(pattern.Name) ? $"{pattern.Name} - " : string.Empty)}{e.ErrorMessage}"));
                    }
                }

                if (errorsOccurred)
                {
                    return await Result<int>.FailAsync(errors);
                }

                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllPatternsCacheKey);
                return await Result<int>.SuccessAsync(result.Data.Count(), result.Messages[0]);
            }
            else
            {
                return await Result<int>.FailAsync(result.Messages);
            }
        }
    }
}
