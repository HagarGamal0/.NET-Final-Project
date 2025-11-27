using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PickGo_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PickGo_backend.Context
{
    public static class DataSeeder
    {

        //public static void SeedUser(this ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>().HasData(
        //        new User { Id = "1111111111", FullName = "Admin", Email="admin@gmail.com" , PasswordHash="Asd@1234"},
        //        new User { Id = "2222222222", FullName = "ali", Email="ali@gmail.com" , PasswordHash="Asd@1234"},
        //        new User { Id = "3333333333", FullName = "ahmed", Email = "ahmed@gmail.com", PasswordHash = "Asd@1234",
        //        }
        //    );
        //}

        public static void SeedRole(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "Customer", NormalizedName = "CUSTOMER" },
                new IdentityRole { Id = "3", Name = "Supplier", NormalizedName = "SUPPLIER" },
                new IdentityRole { Id = "4", Name = "Courier", NormalizedName = "COURIER" }
            );
        }


        public static void SeedAdmin(this ModelBuilder modelBuilder)
        {
            var hasher = new PasswordHasher<User>();

            var admin = new User
            {
                Id = "1",   // Id ثابت
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            admin.PasswordHash = hasher.HashPassword(admin, "123456789"); // كلمة السر

            modelBuilder.Entity<User>().HasData(admin);

            // تأكدي من وجود دور Admin
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "1",   // Id الدور Admin من SeedRole
                    UserId = admin.Id
                }
            );
        }




        public static void SeedSubscription(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subscription>().HasData(
                new Subscription
                {
                    Id = 1,
                    Name = "Basic",
                    Price = 0,
                    UserType = "Courier",
                    MaxOrders = 5,
                    Description = "Basic package for couriers with 5 allowed orders"
                },
                new Subscription
                {
                    Id = 2,
                    Name = "Standard",
                    Price = 50,
                    UserType = "Courier",
                    MaxOrders = 10,
                    Description = "Standard package for couriers with 10 allowed orders"
                },
                new Subscription
                {
                    Id = 3,
                    Name = "Premium",
                    Price = 100,
                    UserType = "Courier",
                    MaxOrders = 20,
                    Description = "Premium package for couriers with 20 allowed orders"
                },
                new Subscription
                {
                    Id = 4,
                    Name = "Basic",
                    Price = 0,
                    UserType = "Supplier",
                    MaxOrders = 5,
                    Description = "Basic package for suppliers with 5 allowed orders"
                },
                new Subscription
                {
                    Id = 5,
                    Name = "Standard",
                    Price = 50,
                    UserType = "Supplier",
                    MaxOrders = 10,
                    Description = "Standard package for suppliers with 10 allowed orders"
                },
                new Subscription
                {
                    Id = 6,
                    Name = "Premium",
                    Price = 100,
                    UserType = "Supplier",
                    MaxOrders = 20,
                    Description = "Premium package for suppliers with 20 allowed orders"
                }
            );
        }

        //public static void SeedSupplier(this ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Supplier>().HasData(
        //        new Supplier
        //        {
        //            UserId = ""3,
        //            ShopName = "Tech Supplies Co.",
        //            ShopLogo = "https://example.com/logo.png",
        //            ShopLocation = "Sahary, Aswan"
        //        });
        //}
        //public static void SeedProduct(this ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Product>().HasData(
        //        new Product { Id = 1, Name = "Laptop", Description = "A high-performance laptop", Price = 999.99m, CategoryId = 1, SupplierId = 3 },
        //        new Product { Id = 2, Name = "Smartphone", Description = "A latest model smartphone", Price = 699.99m, CategoryId = 1, SupplierId = 3 },
        //        new Product { Id = 3, Name = "Tablet", Description = "A best-selling Tablet", Price = 1999.99m, CategoryId = 1, SupplierId = 3 },
        //        new Product { Id = 4, Name = "T-Shirt", Description = "A comfortable cotton t-shirt", Price = 14.99m, CategoryId = 3, SupplierId = 3 },
        //        new Product { Id = 5, Name = "Blender", Description = "A powerful kitchen blender", Price = 49.99m, CategoryId = 4, SupplierId = 3 }
        //    );
        //}
    }

}
