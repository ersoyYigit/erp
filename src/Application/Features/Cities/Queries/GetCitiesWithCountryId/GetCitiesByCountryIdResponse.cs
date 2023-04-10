using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Cities.Queries.GetCitiesWithCountryId
{
    public class GetCitiesByCountryIdResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
}
