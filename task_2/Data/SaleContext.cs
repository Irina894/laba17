using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace P03_SalesDatabase.Data
{
    public class SalesContext : DbContext
    {
        public SalesContext()
        {
        }

        public SalesContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Sale> Sales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            { 
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=P03_SalesDatabase;Integrated Security=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Name)
                      .HasMaxLength(50)
                      .IsUnicode(true);

                entity.Property(e => e.Description)
          .HasMaxLength(250)                 
          .HasDefaultValue("No description");
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.Property(e => e.Name)
                      .HasMaxLength(80)
                      .IsUnicode(true);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Name)
                      .HasMaxLength(100)
                      .IsUnicode(true);

                entity.Property(e => e.Email)
                      .HasMaxLength(80)
                      .IsUnicode(false);
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.Property(e => e.Date)
                      .HasDefaultValueSql("GETDATE()"); 
            });




        }
    }
}