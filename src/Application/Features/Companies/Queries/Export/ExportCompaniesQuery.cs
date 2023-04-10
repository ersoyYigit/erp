using ArdaManager.Application.Extensions;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Application.Interfaces.Services;
using ArdaManager.Application.Specifications.Corporation;
using ArdaManager.Domain.Entities.Corporation;
using ArdaManager.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Companies.Queries.Export
{
    public class ExportCompaniesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportCompaniesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportCompaniesQueryHandler : IRequestHandler<ExportCompaniesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportCompaniesQueryHandler> _localizer;

        public ExportCompaniesQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportCompaniesQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportCompaniesQuery request, CancellationToken cancellationToken)
        {
            var companyFilterSpec = new CompanyFilterSpecification(request.SearchString);
            var companies = await _unitOfWork.Repository<Company>().Entities
                .Specify(companyFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(companies, mappers: new Dictionary<string, Func<Company, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name },
                { _localizer["Code"], item => item.Code },
                { _localizer["Country"], item => item.CountryName },
                { _localizer["City"], item => item.City.Name }
            }, sheetName: _localizer["Companies"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
