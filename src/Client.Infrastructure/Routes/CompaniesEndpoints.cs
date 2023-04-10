using ArdaManager.Application.Features.Companies.Queries.Search;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ArdaManager.Client.Infrastructure.Routes
{
    
    public static class CompaniesEndpoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = BaseEndpoint.Server + $"api/v1/companies?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
            if (orderBy?.Any() == true)
            {
                foreach (var orderByPart in orderBy)
                {
                    url += $"{orderByPart},";
                }
                url = url[..^1]; // loose training ,
            }
            return BaseEndpoint.Server + url;
        }

        public static string GetCount = BaseEndpoint.Server + "api/v1/companies/count";

        public static string GetProductImage(int companyId)
        {
            return BaseEndpoint.Server + $"api/v1/companies/image/{companyId}";
        }

        public static string ExportFiltered(string searchString)
        {
            return BaseEndpoint.Server + $"{Export}?searchString={searchString}";
        }

        public static string Save = BaseEndpoint.Server + "api/v1/companies";
        public static string Delete = BaseEndpoint.Server + "api/v1/companies";
        public static string Export = BaseEndpoint.Server + "api/v1/companies/export";
        public static string ChangePassword = BaseEndpoint.Server + "api/identity/account/changepassword";
        public static string UpdateProfile = BaseEndpoint.Server + "api/identity/account/updateprofile";

        public static string GetFilteredPost = BaseEndpoint.Server + "api/v1/companies/filtered-post";

        public static string GetFiltered(CompanySearchQuery query)
        {

            var queryString = JsonConvert.SerializeObject(query);
            var encodedQueryString = WebUtility.UrlEncode(queryString);
            return BaseEndpoint.Server + $"api/v1/companies/filtered?query={encodedQueryString}";
        }
    }
}
