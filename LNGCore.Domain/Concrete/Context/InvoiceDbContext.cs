using System;
using LNGCore.Domain.Concrete;
using LNGCore.Domain.Concrete.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace LNGCore.Domain.Abstract.Context
{
    public partial class InvoiceDbContext : DbContext, IInvoiceDbContext
    {
        private readonly IConfiguration config;
        public InvoiceDbContext(IConfiguration configParam)
        {
            config = configParam;
        }
        public InvoiceDbContext(DbContextOptions<InvoiceDbContext> options) : base(options)
        {
        }

        public DbSet<BillSheet> BillSheet { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<LineItem> LineItem { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    //.UseLazyLoadingProxies()
                    .UseSqlServer(config["SiteConfiguration:DbContext"]);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BillSheet>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AdditionalInfo).HasMaxLength(500);

                entity.Property(e => e.DueDate).HasMaxLength(500);

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.Login).HasMaxLength(500);

                entity.Property(e => e.PaidBy).HasMaxLength(500);

                entity.Property(e => e.Password).HasMaxLength(500);

                entity.Property(e => e.WhereToPay).HasMaxLength(500);

                entity.Property(e => e.WhoWeOwe).HasMaxLength(500);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AltPhone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Balance)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessPhone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastContact).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PostBox)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Street)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TaxId)
                    .HasColumnName("TaxID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasMany(d => d.Invoice);
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompletedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Deadline).HasColumnType("datetime");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.IsDonated).HasColumnName("isDonated");

                entity.Property(e => e.IsPaid).HasColumnName("isPaid");

                entity.Property(e => e.Notes)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.PaidDate).HasColumnType("datetime");

                entity.Property(e => e.Pofield)
                    .HasColumnName("POField")
                    .HasMaxLength(20);

                entity.Property(e => e.ShipCost).HasColumnName("shipCost");

                entity.Property(e => e.ShippingMethod)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TaxPercent).HasColumnType("decimal(18, 4)");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_Customer");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.Property(e => e.ItemId).HasColumnName("ItemID");

                entity.Property(e => e.ItemName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<LineItem>(entity =>
            {
                entity.Property(e => e.LineItemId).HasColumnName("LineItemID");

                entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");

                entity.Property(e => e.ItemDesc)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ItemId).HasColumnName("ItemID");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.LineItem)
                    .HasForeignKey(d => d.InvoiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LineItem_Invoice");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.LineItem)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LineItem_Item");
            });
        }
    }
}
