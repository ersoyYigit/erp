using ArdaManager.Application.Features.CompanyFairs.Commands.AddEdit;
using ArdaManager.Application.Features.CompanyFairs.Queries.GetAll;
using ArdaManager.Application.Features.CompanyFairs.Queries.GetByCompany;
using ArdaManager.Application.Features.CompanyFairs.Queries.GetById;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Corporation.CompanyFairs
{
    public interface ICompanyFairManager : IManager
    {
        Task<IResult<List<GetAllCompanyFairsResponse>>> GetAllAsync();
        Task<IResult<int>> SaveAsync(AddEditCompanyFairCommand request);
        Task<IResult<int>> DeleteAsync(int id);
        Task<IResult<List<GetCompanyFairByIdResponse>>> GetCompanyFairByIdAsync(GetCompanyFairByIdQuery query);
        Task<IResult<List<GetCompanyFairsByCompanyIdResponse>>> GetCompanyFairsByCompanyIdAsync(GetCompanyFairsByCompanyIdQuery query);

    }
}
