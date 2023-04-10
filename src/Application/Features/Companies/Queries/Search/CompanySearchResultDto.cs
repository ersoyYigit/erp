using ArdaManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Companies.Queries.Search
{
    public class CompanySearchResultDto
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public CompanyType Type { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
