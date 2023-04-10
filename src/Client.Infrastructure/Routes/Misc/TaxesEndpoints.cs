using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Routes
{
    public static class TaxesEndpoints
    {
        public static string GetAll = BaseEndpoint.Server + "api/v1/taxes";
        public static string Delete = BaseEndpoint.Server + "api/v1/taxes";
        public static string Save = BaseEndpoint.Server + "api/v1/taxes";
    }
}
