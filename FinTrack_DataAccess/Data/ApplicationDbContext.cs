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
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    builder.Entity<Budget>().HasKey(e => e.Id);
        //    builder.Entity<Goal>().HasKey(e => e.Id);
        //    builder.Entity<Record>().HasKey(e => e.Id);
        //    builder.Entity<Transaction>().HasKey(e => e.Id);
        //    builder.Entity<User>().HasKey(e => e.Id);

        //    //builder.Entity<Record>()
        //    //.HasKey(r => new { r.Id, r.UserId }); // Define composite key (Id and UserId)

        //    //builder.Entity<Record>()
        //    //    .HasOne(r => r.User) // Define one-to-many relationship (optional)
        //    //    .WithMany(u => u.Records) // User can have many Records (optional)
        //    base.OnModelCreating(builder);
        //}
    }
}



//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//{
//    //optionsBuilder.UseSqlite("Filename=FinTrack.db");
//    base.OnConfiguring(optionsBuilder);
//}

//protected override void OnModelCreating(ModelBuilder builder)
//{
        //    builder.Entity<Budget>().HasKey(e => e.Id);
        //    builder.Entity<Address>().HasKey(e => e.Id);
        //    builder.Entity<Employee>().HasKey(e => e.Id);
        //    builder.Entity<Team>().HasKey(e => e.Id);
        //    builder.Entity<Job>().HasKey(e => e.Id);

//    builder.Entity<Employee>().HasOne(e => e.Address);
//    builder.Entity<Employee>().HasOne(e => e.Job);

//    builder.Entity<Team>().HasMany(e => e.Employees).WithMany(e => e.Teams);

//    base.OnModelCreating(builder);
//}
