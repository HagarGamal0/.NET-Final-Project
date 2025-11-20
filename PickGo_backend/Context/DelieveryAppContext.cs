using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PickGo_backend.Models;
using System.Net;


namespace PickGo_backend.Context
{


    public class DelieveryAppContext : IdentityDbContext<User>
    {
        public DelieveryAppContext(DbContextOptions<DelieveryAppContext> options)
                   : base(options)
        {
        }

        public DbSet<Request> Requests { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // important for Identity tables

            modelBuilder.SeedRole();
            //modelBuilder.SeedSupplier();    
            //modelBuilder.SeedProduct();

        }
    }
}

