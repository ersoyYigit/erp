using ArdaManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Companies.Queries.GetAllPaged
{
    public class GetAllPagedCompaniesResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public CompanyType Type { get; set; }
        public string Address { get; set; }
        public String WebSite { get; set; }
        public string Telephone { get; set; }
        public string Telephone2 { get; set; }
        public string Fax { get; set; }
        public string Vat { get; set; }
        public string Notes { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int? RepresantativeId { get; set; }
        public string RepresantativeName { get; set; }
    }
}
