using ArdaManager.Domain.Entities.Misc;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArdaManager.Domain.Entities.Report.Warehouse;

namespace ArdaManager.Infrastructure.Configurations
{
    public  class WarehouseReportConfiguration : IEntityTypeConfiguration<WarehouseReport>
    {
        public void Configure(EntityTypeBuilder<WarehouseReport> builder)
        { 
        }

    }
}
