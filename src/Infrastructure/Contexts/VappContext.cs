using ArdaManager.Application.Interfaces.Services;
using ArdaManager.Application.Models.Chat;
using ArdaManager.Infrastructure.Models.Identity;
using ArdaManager.Domain.Contracts;
using ArdaManager.Domain.Entities.Catalog;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArdaManager.Domain.Entities.ExtendedAttributes;
using ArdaManager.Domain.Entities.Misc;
using ArdaManager.Domain.Entities.Addressing;
using ArdaManager.Domain.Entities.Corporation;
using ArdaManager.Infrastructure.Configurations;
using ArdaManager.Domain.Entities.Human;
using ArdaManager.Domain.Entities.Inventory;
using System.Reflection.Emit;
using ArdaManager.Domain.Entities;
using ArdaManager.Domain.Enums;
using ArdaManager.Domain.Entities.Transactions;
using ArdaManager.Domain.Entities.Transactions.WarehouseDocs;
using ArdaManager.Domain.Entities.Report.Warehouse;
using ArdaManager.Domain.Entities.Transactions.Purchase;
using ArdaManager.Domain.Entities.Approval;
using ArdaManager.Application.Features.Products.Queries.Search;

namespace ArdaManager.Infrastructure.Contexts
{
    public class VappContext : AuditableContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeService _dateTimeService;

        public VappContext(DbContextOptions<VappContext> options, ICurrentUserService currentUserService, IDateTimeService dateTimeService)
            : base(options)
        {
            _currentUserService = currentUserService;
            _dateTimeService = dateTimeService;
        }

        public DbSet<BaseEntity> BaseEntities { get; set; }
        public DbSet<ChatHistory<VappUser>> ChatHistories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Mold> Molds { get; set; }
        public DbSet<MeasurementUnit> MeasurementUnits { get; set; }
        public DbSet<Pattern> Patterns { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<DocumentExtendedAttribute> DocumentExtendedAttributes { get; set; }

        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Category> Categories { get; set; }




        public DbSet<Person> Persons { get; set; }
        public DbSet<Fair> Fairs { get; set; }
        public DbSet<CompanyFair> CompanyFairs { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Rack> Racks { get; set; }
        public DbSet<RecipeItem> RecipeItems { get; set; }
        public DbSet<BaseDoc> BaseDocs { get; set; }
        public DbSet<ProductionLine> ProductionLines { get; set; }
        public DbSet<TemplateWorkOrder> TemplateWorkOrders { get; set; }

        public DbSet<WarehouseReceipt> WarehouseReceipts { get; set; }
        public DbSet<WarehouseReceiptRow> WarehouseReceiptRows { get; set; }

        public DbSet<PurchaseRequest> PurchaseRequests { get; set; }
        public DbSet<PurchaseRequestRow> PurchaseRequestRows { get; set; }
        public DbSet<PurchaseOffer> PurchaseOffers { get; set; }
        public DbSet<PurchaseOfferRow> PurchaseOfferRows { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderRow> PurchaseOrderRows { get; set; }

        public DbSet<ApprovalScenario> ApprovalScenarios { get; set; }

        public DbSet<ApprovalStep> ApprovalSteps { get; set; }

        public DbSet<DocumentApprovalStatus> DocumentApprovalStatuses { get; set; }


        public DbSet<Currency> Currencies { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public DbSet<Tax> Taxes { get; set; }


        /*REPORTING*/
        public DbSet<WarehouseReport> WarehouseReports { get; set; }
        public DbSet<ProductSearchResultDto> ProductSearchResultDtos { get; set; }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = _dateTimeService.NowUtc;
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = _dateTimeService.NowUtc;
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        break;
                }
            }
            if (_currentUserService.UserId == null)
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            else
            {
                return await base.SaveChangesAsync(_currentUserService.UserId, cancellationToken);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.ApplyConfiguration(new CompanyConfiguration());
            builder.ApplyConfiguration(new CityConfiguration());
            builder.ApplyConfiguration(new CompanyFairConfiguration());
            builder.ApplyConfiguration(new RecipeItemConfiguration());
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new WarehouseReceiptRowConfiguration());
            builder.ApplyConfiguration(new PurchaseRequestRowConfiguration());
            builder.ApplyConfiguration(new PurchaseOfferRowConfiguration());
            builder.ApplyConfiguration(new PurchaseOrderRowConfiguration());
            builder.ApplyConfiguration(new PurchaseOfferConfiguration());
            builder.ApplyConfiguration(new PurchaseOrderConfiguration());


            builder.ApplyConfiguration(new ExchangeRateConfiguration());
            builder.ApplyConfiguration(new BaseDocConfiguration());


            /*REPORTING*/
            builder.Entity<WarehouseReport>().HasNoKey().ToView("vw_warehouses_stocks");
            builder.Entity<ProductSearchResultDto>().HasNoKey().ToSqlQuery("sp_search_products_with_stocks");
            /*REPORTING*/
            


            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?) ))
            {
                //property.SetColumnType("decimal(18,2)");
            }
            base.OnModelCreating(builder);
            builder.Entity<ChatHistory<VappUser>>(entity =>
            {
                entity.ToTable("ChatHistory");

                entity.HasOne(d => d.FromUser)
                    .WithMany(p => p.ChatHistoryFromUsers)
                    .HasForeignKey(d => d.FromUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.ChatHistoryToUsers)
                    .HasForeignKey(d => d.ToUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
            builder.Entity<VappUser>(entity =>
            {
                entity.ToTable(name: "Users", "Identity");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            builder.Entity<VappRole>(entity =>
            {
                entity.ToTable(name: "Roles", "Identity");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles", "Identity");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims", "Identity");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins", "Identity");
            });

            builder.Entity<VappRoleClaim>(entity =>
            {
                entity.ToTable(name: "RoleClaims", "Identity");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleClaims)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens", "Identity");
            });







        }
    }
}