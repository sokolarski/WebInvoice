using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebInvoice.Data.AppData.Models;
using WebInvoice.Data.CompanyData.Models;
using WebInvoice.Data.Repository;
using WebInvoice.Data.Repository.Models;

namespace WebInvoice.Data
{
    public class CompanyDbContext : DbContext
    {
        private static readonly MethodInfo SetIsDeletedQueryFilterMethod =
            typeof(ApplicationDbContext).GetMethod(
                nameof(SetIsDeletedQueryFilter),
                BindingFlags.NonPublic | BindingFlags.Static);

        public CompanyDbContext()
        {

        }
        public CompanyDbContext(DbContextOptions<CompanyDbContext> options)
            : base(options)
        {

        }

        public DbSet<BankAccount> BankAccounts { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<CompanyObject> CompanyObjects { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<NonVatDocument> NonVatDocuments { get; set; }

        public DbSet<Partner> Partners { get; set; }

        public DbSet<PaymentType> PaymentTypes { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<QuantityType> QuantityTypes { get; set; }

        public DbSet<Reason> Reasons { get; set; }

        public DbSet<Sales> Sales { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<VatDocument> VatDocuments { get; set; }

        public DbSet<VatType> VatTypes { get; set; }


        public override int SaveChanges() => this.SaveChanges(true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            this.SaveChangesAsync(true, cancellationToken);

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Needed for Identity models configuration
            base.OnModelCreating(builder);

            ConfigureRelations(builder);
            ConfigureDecimalPrecision(builder);
            EntityIndexesConfiguration.Configure(builder);

            var entityTypes = builder.Model.GetEntityTypes().ToList();

            // Set global query filter for not deleted entities only
            var deletableEntityTypes = entityTypes
                .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
            foreach (var deletableEntityType in deletableEntityTypes)
            {
                var method = SetIsDeletedQueryFilterMethod.MakeGenericMethod(deletableEntityType.ClrType);
                method.Invoke(null, new object[] { builder });
            }

            // Disable cascade delete
            var foreignKeys = entityTypes
                .SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));
            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        private static void ConfigureDecimalPrecision(ModelBuilder builder)
        {
            foreach (var property in builder.Model.GetEntityTypes()
                    .SelectMany(t => t.GetProperties())
                    .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
             
                property.SetPrecision(15);
                property.SetScale(5);
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=CompanyDefaultDb;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
            base.OnConfiguring(optionsBuilder);
        }

        private static void ConfigureRelations(ModelBuilder builder)
        {
            builder.Entity<VatDocument>()
                .Property(e => e.Id)
                .ValueGeneratedNever();

            builder.Entity<VatDocument>()
                .HasOne(e => e.WriterEmployee)
                .WithMany(d => d.VatDocumentsWriter);

            builder.Entity<VatDocument>()
                .HasOne(e => e.RecipientEmployee)
                .WithMany(d => d.VatDocumentsRecipient);

            builder.Entity<NonVatDocument>()
                .Property(e => e.Id)
                .ValueGeneratedNever();

            builder.Entity<NonVatDocument>()
                .HasOne(e => e.WriterEmployee)
                .WithMany(d => d.NonVatDocumentsWriter);

            builder.Entity<NonVatDocument>()
                .HasOne(e => e.RecipientEmployee)
                .WithMany(d => d.NonVatDocumentsRecipient);

            builder.Entity<CompanyObject>()
                .HasIndex(e => e.Name).IsUnique();

            builder.Entity<PaymentType>()
                .HasIndex(e => e.Name).IsUnique();

            builder.Entity<Reason>()
                .HasIndex(e => e.Name).IsUnique();

            builder.Entity<VatType>()
                .HasIndex(e => e.Name).IsUnique();

            builder.Entity<QuantityType>()
                .HasIndex(e => e.Type).IsUnique();

            builder.Entity<Product>()
                .HasIndex(e => e.Name).IsUnique();

            builder.Entity<Company>()
                .HasIndex(e => e.Name).IsUnique();

            builder.Entity<Partner>()
                .HasIndex(e => e.Name).IsUnique();
        }

        private static void SetIsDeletedQueryFilter<T>(ModelBuilder builder)
            where T : class, IDeletableEntity
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }

        private void ApplyAuditInfoRules()
        {
            var changedEntries = this.ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is IAuditInfo &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in changedEntries)
            {
                var entity = (IAuditInfo)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == default)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                }
                else
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                }
            }
        }
    }
}
