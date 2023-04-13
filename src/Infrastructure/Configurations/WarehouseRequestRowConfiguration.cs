using ArdaManager.Domain.Entities.Transactions.WarehouseDocs;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Infrastructure.Configurations
{
    public  class WarehouseRequestRowConfiguration : IEntityTypeConfiguration<WarehouseRequestRow>
    {
        public void Configure(EntityTypeBuilder<WarehouseRequestRow> builder)
        {

            builder
            .HasOne(c => c.Product)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);


            builder
            .HasOne(c => c.Rack)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);


        }
    }

    public class WarehouseRequestConfiguration : IEntityTypeConfiguration<WarehouseRequest>
    {
        public void Configure(EntityTypeBuilder<WarehouseRequest> builder)
        {

            builder
            .HasOne(c => c.Warehouse)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);



        }
    }
}
