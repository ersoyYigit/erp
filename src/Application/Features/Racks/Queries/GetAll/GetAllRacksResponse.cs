using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Racks.Queries.GetAll
{
    public class GetAllRacksResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Section { get; set; }
        public string SectionCode { get; set; }
        public string Description { get; set; }
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
    }
}
