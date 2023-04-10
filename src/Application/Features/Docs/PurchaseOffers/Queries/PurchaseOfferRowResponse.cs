using ArdaManager.Domain.Entities.Misc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Features.Docs.PurchaseOffers.Queries
{
    public class PurchaseOfferRowResponse
    {

        public int Id { get; set; }
        public int PurchaseOfferId { get; set; }


        public decimal Quantity { get; set; }
        public string Description { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int MeasurementUnitId { get; set; }
        public string MeasurementUnitCode { get; set; }
        public int RowNo { get; set; }
        public string MeasurementUnitName { get; set; }
        public string MeasurementUnitSystem { get; set; }
        public int? PrevRowId { get; set; }
        public int? PurchaseRequestRowId { get; set; }


        /* PRICE PROPERTIES*/
        public decimal Price { get; set; } = decimal.Zero;
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }

        public decimal ExchangeRate { get; set; } = decimal.Zero;
        public int TaxId { get; set; }
        public string TaxCode { get; set; }

        public decimal TaxAmount { get; set; } = decimal.Zero;

        public decimal DiscountPercentage { get; set; }

        public decimal DiscountAmount { get; set; } = decimal.Zero;

        public decimal TotalAmount { get; set; } = decimal.Zero;

        public virtual Currency Currency { get; set; }
        public virtual Tax Tax { get; set; }
        /* PRICE PROPERTIES*/
    }
}
