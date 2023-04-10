using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Routes
{
    public static class CompanyFairsEndpoints
    {
        public static string GetAll()
        {
            return BaseEndpoint.Server + $"api/v1/companyfairs";
        }

        public static string GetById(int id)
        {
            return BaseEndpoint.Server + $"api/v1/companyfairs/{id}";
        }


        public static string GetAllByCompanyIdId(int companyid)
        {
            return BaseEndpoint.Server + $"api/v1/companyfairs/GetByCompanyId/{companyid}";
        }
        
        public static string Save = BaseEndpoint.Server + "api/v1/companyfairs";
        public static string Delete = BaseEndpoint.Server + "api/v1/companyfairs";
    }
}
