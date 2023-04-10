using ArdaManager.Domain.Contracts;
using ArdaManager.Domain.Entities.Addressing;
using ArdaManager.Domain.Entities.Human;
using ArdaManager.Domain.Entities.Misc;
using ArdaManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Entities.Corporation
{
    [Table("Companies")]
    public class Company : BaseEntity
    {



        public CompanyType Type { get; set; }
        public string Address { get; set; }
        public string WebSite { get; set; }
        public string Telephone { get; set; }
        public string Telephone2 { get; set; }
        public string Fax { get; set; }
        public string Vat { get; set; }
        public string Notes { get; set; }
        public string MeetPlace { get; set; }
        public DateTime? MeetDate { get; set; }
        public string TradeRegistry { get; set; }
        public string Bank { get; set; }
        public string Consignee { get; set; }
        public string ConsigneeAddress { get; set; }
        public string InfoMail { get; set; }
        public bool IsActive { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }



        //not mappad
        public int? CountryId { get; set; }
        public string CountryName { get; set; }

        public int? CityId { get; set; }
        public int? CategoryId { get; set; }
        public int? PaymentMethodId { get; set; }
        public int? RepresantativeId { get; set; }

        public virtual City City { get; set; }
        public virtual Category Category { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
        public virtual Person Represantative { get; set; }

    }
}
