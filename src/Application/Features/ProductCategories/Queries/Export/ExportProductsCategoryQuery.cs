using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ArdaManager.Application.Extensions;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Application.Interfaces.Services;
using ArdaManager.Application.Specifications.Catalog;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ArdaManager.Application.Features.ProductCategories.Queries.Export
{
    
    public class ExportProductCategoriesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportProductCategoriesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportProductCategoriesQueryHandler : IRequestHandler<ExportProductCategoriesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportProductCategoriesQueryHandler> _localizer;

        public ExportProductCategoriesQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportProductCategoriesQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportProductCategoriesQuery request, CancellationToken cancellationToken)
        {
            var patternFilterSpec = new ProductCategoryFilterSpecification(request.SearchString);
            var patterns = await _unitOfWork.Repository<ProductCategory>().Entities
                .Specify(patternFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(patterns, mappers: new Dictionary<string, Func<ProductCategory, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name },
                { _localizer["Description"], item => item.Description }
            }, sheetName: _localizer["ProductCategories"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
