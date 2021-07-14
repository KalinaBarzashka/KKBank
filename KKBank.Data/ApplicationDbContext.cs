using KKBank.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KKBank.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        
        public virtual DbSet<AccountType> AccountTypes { get; set; }
        
        public virtual DbSet<Currency> Currency { get; set; }
        
        public virtual DbSet<PaymentOrderStatus> PaymentOrderStatus { get; set; }

        public virtual DbSet<PaymentOrder> PaymentOrders { get; set; }

        public virtual DbSet<AccountRequest> AccountRequests { get; set; }

        public virtual DbSet<RequestType> RequestTypes { get; set; }

        public virtual DbSet<Event> Events { get; set; }

        public virtual DbSet<AccountRequestStatus> AccountRequestStatus { get; set; }

        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.;Database=KKBank;Integrated Security=True");
            }
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Needed for Identity models configuration
            base.OnModelCreating(builder);

            this.ConfigureUserIdentityRelations(builder);
            
            var entityTypes = builder.Model.GetEntityTypes().ToList();
            
            // Disable cascade delete
            var foreignKeys = entityTypes
                .SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));

            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
            
            builder.Entity<Account>().ToTable("Accounts", "17118069");
            builder.Entity<Account>().Property(x => x.Id).UseIdentityColumn();
            
            builder.Entity<AccountType>().ToTable("AccountTypes", "17118069");
            builder.Entity<AccountType>().Property(x => x.Id).UseIdentityColumn();
            
            builder.Entity<Currency>().ToTable("Currency", "17118069");
            builder.Entity<Currency>().Property(x => x.Id).UseIdentityColumn();
            
            builder.Entity<PaymentOrderStatus>().ToTable("PaymentOrderStatus", "17118069");
            builder.Entity<PaymentOrderStatus>().Property(x => x.Id).UseIdentityColumn();

            builder.Entity<PaymentOrder>().ToTable("PaymentOrders", "17118069");
            builder.Entity<PaymentOrder>().Property(x => x.Id).UseIdentityColumn();

            builder.Entity<RequestType>().ToTable("RequestTypes", "17118069");
            builder.Entity<RequestType>().Property(x => x.Id).UseIdentityColumn();

            builder.Entity<AccountRequestStatus>().ToTable("AccountRequestStatus", "17118069");
            builder.Entity<AccountRequestStatus>().Property(x => x.Id).UseIdentityColumn();

            builder.Entity<AccountRequest>().ToTable("AccountRequests", "17118069");
            builder.Entity<AccountRequest>().Property(x => x.Id).UseIdentityColumn();

            builder.Entity<Event>().ToTable("log_17118069", "17118069");
            builder.Entity<Event>().Property(x => x.Id).UseIdentityColumn();

            builder.Entity<Account>(entity =>
            {
                entity.HasMany(x => x.PaymentOrdersList)
                      .WithOne(x => x.FromAccount)
                      .HasForeignKey(x => x.FromAccountId);
            
                entity.HasMany(x => x.IncomesList)
                      .WithOne(x => x.ToAccount)
                      .HasForeignKey(x => x.ToAccountId);
            });
            
            builder.Entity<Account>().Ignore(x => x.Balance);


        }

        private void ConfigureUserIdentityRelations(ModelBuilder builder)
            => builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
