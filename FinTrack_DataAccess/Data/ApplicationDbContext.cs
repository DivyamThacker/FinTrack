using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
namespace FinTrack_DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
             
        }
        public DbSet<Budget> Budgets { get; set; }
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
