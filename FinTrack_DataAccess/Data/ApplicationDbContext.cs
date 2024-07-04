using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
namespace FinTrack_DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<SavingsAccount> SavingsAccounts { get; set; }
        public DbSet<InvestmentAccount> InvestmentAccounts { get; set; }
        public DbSet<Holding> Holdings { get; set; }
        public DbSet<MarketData> MarketData { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>()
                .HasDiscriminator<string>("Type")
                .HasValue<SavingsAccount>("Savings")
                .HasValue<InvestmentAccount>("Investment");

            modelBuilder.Entity<Account>()
           .Property(a => a.Balance)
           .HasPrecision(18, 2); // Adjust the precision and scale as needed

            //Account - Transaction (One to many)
             modelBuilder.Entity<SavingsAccount>()
                 .HasMany(a => a.Transactions)
                 .WithOne(t => t.SavingsAccount)
                 .HasForeignKey(t => t.AccountId);

            //Account - Record (One to many)
            modelBuilder.Entity<SavingsAccount>()
                .HasMany(a => a.Records)
                .WithOne(r => r.SavingsAccount)
                .HasForeignKey(r => r.AccountId);

            //Account - Budget (One to many)
            modelBuilder.Entity<SavingsAccount>()
                .HasMany(a => a.Budgets)
                .WithOne(b => b.SavingsAccount)
                .HasForeignKey(b => b.AccountId);

            //Account - Goal (One to many)
            modelBuilder.Entity<SavingsAccount>()
                .HasMany(a => a.Goals)
                .WithOne(g => g.SavingsAccount)
                .HasForeignKey(g => g.AccountId);

            // Configure relationships and indexes
            modelBuilder.Entity<Account>()
                .HasKey(a => a.Id); // Primary key

            modelBuilder.Entity<Account>()
            .Property(a => a.Id)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<Account>()
                .HasIndex(a => new { a.UserId, a.Name })
                .IsUnique(); // Unique constraint on UserId and Name
            
            // Account - User (many-to-one)
            modelBuilder.Entity<Account>()
                .HasOne(a => a.User)
                .WithMany(u => u.Accounts)
                .HasForeignKey(a => a.UserId); // Foreign key

            // Holding - Account (many-to-one)
            modelBuilder.Entity<Holding>()
                .HasOne(h => h.InvestmentAccount)
                .WithMany(a => a.Holdings)
                .HasForeignKey(h => h.AccountId);

            // Holding - User (many-to-one)
            modelBuilder.Entity<Holding>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

             //Trade - Account(many - to - one)
            modelBuilder.Entity<Trade>()
                .HasOne(t => t.InvestmentAccount)
                .WithMany(a => a.Trades)
                .HasForeignKey(t => t.AccountId);

                // Trade - User (many-to-one)
                modelBuilder.Entity<Trade>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Record - User (many-to-one)
            modelBuilder.Entity<Record>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            // Budget - User (many-to-one)
            modelBuilder.Entity<Budget>()
               .HasOne<ApplicationUser>()
               .WithMany()
               .HasForeignKey(b => b.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            // Goal - User (many-to-one)
            modelBuilder.Entity<Goal>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MarketData>().HasKey(e => e.Symbol);
        }
    }
}

