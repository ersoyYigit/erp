using ArdaManager.Application.Features.CompanyFairs.Commands.AddEdit;
using ArdaManager.Application.Features.CompanyFairs.Queries.GetAll;
using ArdaManager.Application.Features.CompanyFairs.Queries.GetByCompany;
using ArdaManager.Application.Features.CompanyFairs.Queries.GetById;
using ArdaManager.Client.Infrastructure.Extensions;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Corporation.CompanyFairs
{
    public class CompanyFairManager : ICompanyFairManager
    {
        private readonly HttpClient _httpClient;

        public CompanyFairManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<List<GetAllCompanyFairsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.CompanyFairsEndpoints.GetAll());
            return await response.ToResult<List<GetAllCompanyFairsResponse>>();
        }

        public async Task<IResult<List<GetCompanyFairByIdResponse>>> GetCompanyFairByIdAsync(GetCompanyFairByIdQuery query)
        {
            var response = await _httpClient.GetAsync(Routes.CompanyFairsEndpoints.GetById(query.Id));
            return await response.ToResult<List<GetCompanyFairByIdResponse>>();
        }

        public async Task<IResult<List<GetCompanyFairsByCompanyIdResponse>>> GetCompanyFairsByCompanyIdAsync(GetCompanyFairsByCompanyIdQuery query)
        {
            var response = await _httpClient.GetAsync(Routes.CompanyFairsEndpoints.GetAllByCompanyIdId(query.CompanyId));
            return await response.ToResult<List<GetCompanyFairsByCompanyIdResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditCompanyFairCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.CompanyFairsEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.CompanyFairsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

    }
}
