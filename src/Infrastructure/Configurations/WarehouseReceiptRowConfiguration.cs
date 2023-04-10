using ArdaManager.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArdaManager.Application.Mappings.Docs;
using ArdaManager.Domain.Entities.Transactions.WarehouseDocs;

namespace ArdaManager.Infrastructure.Configurations
{
    public  class WarehouseReceiptRowConfiguration : IEntityTypeConfiguration<WarehouseReceiptRow>
    {
        public void Configure(EntityTypeBuilder<WarehouseReceiptRow> builder)
        {

            builder
            .HasOne(c => c.Product)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
            
            builder
            .HasOne(c => c.Currency)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            builder
            .HasOne(c => c.Tax)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);


        }
    }
}
