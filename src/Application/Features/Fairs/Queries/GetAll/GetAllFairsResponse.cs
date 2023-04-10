using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Fairs.Queries.GetAll
{
    public class GetAllFairsResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }
        
    }

}
