using ArdaManager.Domain.Entities.Transactions.WarehouseDocs;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArdaManager.Domain.Entities.Transactions.Purchase;

namespace ArdaManager.Infrastructure.Configurations
{
    public class PurchaseRequestRowConfiguration : IEntityTypeConfiguration<PurchaseRequestRow>
    {
        public void Configure(EntityTypeBuilder<PurchaseRequestRow> builder)
        {

            builder
            .HasOne(c => c.Product)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);


        }
    }

    public class PurchaseOfferRowConfiguration : IEntityTypeConfiguration<PurchaseOfferRow>
    {
        public void Configure(EntityTypeBuilder<PurchaseOfferRow> builder)
        {

            builder
            .HasOne(c => c.Product)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
            builder
            .HasOne(c => c.Currency)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            builder
            .HasOne(c => c.Tax)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

        }
    }

    public class PurchaseOrderRowConfiguration : IEntityTypeConfiguration<PurchaseOrderRow>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrderRow> builder)
        {

            builder
            .HasOne(c => c.Product)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            builder
            .HasOne(c => c.Currency)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            builder
            .HasOne(c => c.Tax)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }


    public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
        {
            builder
            .HasOne(c => c.Currency)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }

    public class PurchaseOfferConfiguration : IEntityTypeConfiguration<PurchaseOffer>
    {
        public void Configure(EntityTypeBuilder<PurchaseOffer> builder)
        {
            builder
            .HasOne(c => c.Currency)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);


        }
    }

}