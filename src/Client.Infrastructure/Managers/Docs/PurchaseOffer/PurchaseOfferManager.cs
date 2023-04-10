using ArdaManager.Application.Features.Docs.PurchaseOffers.Commands;
using ArdaManager.Application.Features.Docs.PurchaseOffers.Queries;
using ArdaManager.Client.Infrastructure.Extensions;
using ArdaManager.Client.Infrastructure.Routes.Docs;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Managers.Docs.PurchaseOffer
{
    public class PurchaseOfferManager : IPurchaseOfferManager
    {
        private readonly HttpClient _httpClient;

        public PurchaseOfferManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{PurchaseOffersEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<PurchaseOfferResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(PurchaseOffersEndpoints.GetAll);
            return await response.ToResult<List<PurchaseOfferResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(UpsertPurchaseOfferCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(PurchaseOffersEndpoints.Save, request);
            return await response.ToResult<int>();
        }
        public async Task<IResult<int>> ChooseAsync(ChoosePurchaseOfferCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(PurchaseOffersEndpoints.Choose, request);
            return await response.ToResult<int>();
        }
    }
}
