using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PickGo_backend.Configurations;
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
        public DbSet<Courier> Couriers { get; set; }
        public DbSet<CourierLocation>  CourierLocations { get; set; }
        public DbSet<CourierTransaction>  CourierTransactions { get; set; }
        public DbSet<Customer> customers { get; set; }

        public DbSet<DeliveryProof>  deliveryProofs { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Courier> Suppliers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // important for Identity tables

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RequestConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new PackageConfiguration());
            modelBuilder.ApplyConfiguration(new SupplierConfiguration());
            modelBuilder.ApplyConfiguration(new CourierConfiguration());
            modelBuilder.ApplyConfiguration(new CourierLocationConfiguration());
            modelBuilder.ApplyConfiguration(new CourierTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new DeliveryProofConfiguration());
            modelBuilder.ApplyConfiguration(new InvoiceConfiguration());









            modelBuilder.SeedRole();
            modelBuilder.SeedSubscription(); 

            //modelBuilder.SeedSupplier();    
            //modelBuilder.SeedProduct();

        }
    }
}

