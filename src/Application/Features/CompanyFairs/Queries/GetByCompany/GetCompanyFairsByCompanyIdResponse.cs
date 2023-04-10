using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.CompanyFairs.Queries.GetByCompany
{
    public class GetCompanyFairsByCompanyIdResponse
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int FairId { get; set; }
        public string FairName { get; set; }
        public string FairDescription { get; set; }
        public string Type { get; set; }
        public DateTime? FairDate { get; set; }
    }
}
