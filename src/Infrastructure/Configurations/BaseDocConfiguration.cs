using ArdaManager.Domain.Entities.Misc;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArdaManager.Domain.Entities;

namespace ArdaManager.Infrastructure.Configurations
{
    public class BaseDocConfiguration : IEntityTypeConfiguration<BaseDoc>
    {
        public void Configure(EntityTypeBuilder<BaseDoc> builder)
        {
            builder
                .HasOne(bd => bd.NextDoc)
                .WithOne(bd => bd.PrevDoc)
                .HasForeignKey<BaseDoc>(bd => bd.NextDocId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            builder
                .HasOne(bd => bd.PrevDoc)
                .WithOne(bd => bd.NextDoc)
                .HasForeignKey<BaseDoc>(bd => bd.PrevDocId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }
}
