using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Cities.Queries.GetById
{
    public class GetCityByIdResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public string CountryId { get; set; }
        public string CountryName { get; set; }
    }
}
