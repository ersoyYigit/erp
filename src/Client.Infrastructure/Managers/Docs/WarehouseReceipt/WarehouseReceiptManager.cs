using ArdaManager.Application.Features.Docs.WarehouseReceipts.Commands.AddEdit;
using ArdaManager.Application.Features.Docs.WarehouseReceipts.Queries.GetAll;
using ArdaManager.Application.Features.Docs.WarehouseReceipts.Queries.GetById;
using ArdaManager.Client.Infrastructure.Managers.Docs.WarehouseReceipt;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ArdaManager.Client.Infrastructure.Routes.Docs;
using ArdaManager.Client.Infrastructure.Extensions;
using System.Net.Http.Json;
using ArdaManager.Domain.Enums.Doc;

namespace ArdaManager.Client.Infrastructure.Managers.Docs.WarehouseReceipt
{
    public class WarehouseReceiptManager : IWarehouseReceiptManager
    {
        private readonly HttpClient _httpClient;

        public WarehouseReceiptManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }



        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{WarehouseReceiptsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            throw new NotImplementedException();
        }



        public async Task<IResult<List<GetAllWarehouseReceiptsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(WarehouseReceiptsEndpoints.GetAll);
            return await response.ToResult<List<GetAllWarehouseReceiptsResponse>>();
        }

        public async Task<IResult<List<GetAllWarehouseReceiptsResponse>>> GetAllByType(WarehouseReceiptType type)
        {

            var response = await _httpClient.GetAsync(WarehouseReceiptsEndpoints.GetAllByType(type));
            return await response.ToResult<List<GetAllWarehouseReceiptsResponse>>();
        }



        public async Task<IResult<List<GetAllWarehouseReceiptsResponse>>> GetAllByWarehouseId(WarehouseReceiptType type, int id)
        {
            var response = await _httpClient.GetAsync(WarehouseReceiptsEndpoints.GetAllByWarehouseId(type,id));
            return await response.ToResult<List<GetAllWarehouseReceiptsResponse>>();
        }

        public async Task<IResult<GetAllWarehouseReceiptsResponse>> GetByIdAsync()
        {
            var response = await _httpClient.GetAsync(WarehouseReceiptsEndpoints.GetAll);
            return await response.ToResult<GetAllWarehouseReceiptsResponse>();
        }

        public async Task<IResult<GetWarehouseReceiptByIdResponse>> GetByIdAsync(GetWarehouseReceiptByIdQuery query)
        {
            var response = await _httpClient.GetAsync(WarehouseReceiptsEndpoints.GetById(query.Id));
            return await response.ToResult<GetWarehouseReceiptByIdResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditWarehouseReceiptCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(WarehouseReceiptsEndpoints.Save, request);
            return await response.ToResult<int>();
        }

    }
}
