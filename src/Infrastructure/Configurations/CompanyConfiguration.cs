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
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            
            builder.Ignore(x => x.CountryId);
            builder.Ignore(x => x.CountryName);


            /*
            builder.HasOne(c => c.City)
                .WithMany()
                .HasForeignKey("CityId")
                .OnDelete(DeleteBehavior.ClientCascade);
            */
        }
    }
}
