using ArdaManager.Application.Features.Companies.Commands.AddEdit;
using ArdaManager.Application.Features.Companies.Queries.GetAllPaged;
using ArdaManager.Application.Features.Companies.Queries.Search;
using ArdaManager.Application.Features.Products.Queries.Search;
using ArdaManager.Application.Requests.Corporation;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Corporation.Company
{
    public interface ICompanyManager : IManager
    {
        Task<PaginatedResult<GetAllPagedCompaniesResponse>> GetCompaniesAsync(GetAllPagedCompaniesRequest request);

        //Task<IResult<string>> GetCompanyImageAsync(int id);

        Task<IResult<int>> SaveAsync(AddEditCompanyCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");

        Task<IResult<List<CompanySearchResultDto>>> GetFilteredAsync(CompanySearchQuery query);
    }
}
