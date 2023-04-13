using ArdaManager.Domain.Entities.Report.Purchase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Reports.Purchase
{
    public class GetPurchaseProcessResponse : PurchaseProcessResult
    {
        public List<PurchaseProcessRowResult> PurchaseProcessRowsResult { get; set; }
    }
}
