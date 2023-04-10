using ArdaManager.Domain.Entities.Addressing;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArdaManager.Domain.Entities.Catalog;
using System.Reflection.Emit;

namespace ArdaManager.Infrastructure.Configurations
{
    public class RecipeItemConfiguration : IEntityTypeConfiguration<RecipeItem>
    {
        public void Configure(EntityTypeBuilder<RecipeItem> builder)
        {

            builder
                .HasOne(c => c.OwnerProduct)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(c => c.RecipeProduct)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
