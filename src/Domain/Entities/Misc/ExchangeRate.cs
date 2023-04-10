using ArdaManager.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Entities.Misc
{
    public class ExchangeRate : AuditableEntity<int>
    {
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public DateTime RateDate { get; set; }

        [Column(TypeName = "decimal(18,8)")]
        public decimal ForexBuying { get; set; }
        [Column(TypeName = "decimal(18,8)")]
        public decimal ForexSelling { get; set; }
        [Column(TypeName = "decimal(18,8)")]
        public decimal BanknoteBuying { get; set; }
        [Column(TypeName = "decimal(18,8)")]
        public decimal BanknoteSelling { get; set; }
    }

}
