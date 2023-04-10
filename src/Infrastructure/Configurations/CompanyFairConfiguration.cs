using ArdaManager.Domain.Entities.Corporation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Infrastructure.Configurations
{
    public class CompanyFairConfiguration : IEntityTypeConfiguration<CompanyFair>
    {
        public void Configure(EntityTypeBuilder<CompanyFair> builder)
        {

            builder
                .HasIndex(p => new { p.CompanyId, p.FairId })
                .IsUnique(true);


        }
    }
}
