using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Routes
{
    public static class CitiesEndpoints
    {

        public static string GetAll()
        {
            return BaseEndpoint.Server + $"api/v1/city";
        }

        public static string GetById(int cityId)
        {
            return BaseEndpoint.Server + $"api/v1/city/{cityId}";
        }


        public static string GetByCountryIdAll(int countryId)
        { 
            return BaseEndpoint.Server + $"api/v1/city/GetByCountryId/{countryId}";
        }
    }
}
