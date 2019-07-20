using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace LNGCore.Domain.Database
{
    public partial class LngDbContext : DbContext
    {
        private readonly IConfiguration _config;
        public LngDbContext(IConfiguration configParam)
        {
            _config = configParam;
        }

        public LngDbContext(DbContextOptions<LngDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Configuration> Configuration { get; set; }
        public virtual DbSet<BillSheet> BillSheet { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<Guestlist> Guestlist { get; set; }
        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<InvoiceAttachment> InvoiceAttachment { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<LineItem> LineItem { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    //.UseLazyLoadingProxies()
                    .UseSqlServer(_config.GetSection("SiteConfiguration")["DbContext"]);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<BillSheet>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AdditionalInfo).HasMaxLength(500);

                entity.Property(e => e.CreditLimit).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DueDate).HasMaxLength(500);

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.LeftToPay).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Login).HasMaxLength(500);

                entity.Property(e => e.PaidBy).HasMaxLength(500);

                entity.Property(e => e.Password).HasMaxLength(500);

                entity.Property(e => e.PayAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.WhereToPay).HasMaxLength(500);

                entity.Property(e => e.WhoWeOwe).HasMaxLength(500);
            });

            modelBuilder.Entity<Configuration>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Value).HasMaxLength(500);
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

                entity.Property(e => e.SecondaryEmail).HasMaxLength(50);

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
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.EmpName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.EventDate).HasColumnType("datetime");

                entity.Property(e => e.EventDescription).IsRequired();

                entity.Property(e => e.EventName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Event)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Event__EmployeeI__3E1D39E1");
            });

            modelBuilder.Entity<Guestlist>(entity =>
            {
                entity.HasKey(e => e.GuestId);

                entity.ToTable("guestlist");

                entity.Property(e => e.GuestId).HasColumnName("GuestID");

                entity.Property(e => e.Children).HasMaxLength(200);

                entity.Property(e => e.City).HasMaxLength(500);

                entity.Property(e => e.Email).HasMaxLength(200);

                entity.Property(e => e.FirstName).HasMaxLength(200);

                entity.Property(e => e.Guests).HasMaxLength(200);

                entity.Property(e => e.LastName).HasMaxLength(200);

                entity.Property(e => e.Notes).HasMaxLength(500);

                entity.Property(e => e.Phone).HasMaxLength(200);

                entity.Property(e => e.State).HasMaxLength(200);

                entity.Property(e => e.Street).HasMaxLength(200);

                entity.Property(e => e.TotalGuests).HasMaxLength(500);

                entity.Property(e => e.WhoIsWho).HasMaxLength(500);

                entity.Property(e => e.Zip).HasMaxLength(200);
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

                entity.Property(e => e.ShipCost)
                    .HasColumnName("shipCost")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ShippingMethod)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TaxPercent).HasColumnType("decimal(18, 4)");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_Customer");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Invoice_Employee");
            });

            modelBuilder.Entity<InvoiceAttachment>(entity =>
            {
                entity.Property(e => e.AttachmentLocation)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.InverseInvoice)
                    .HasForeignKey(d => d.InvoiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InvoiceAttachments_InvoiceAttachments");
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

                entity.Property(e => e.ItemPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

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

            modelBuilder.Entity<Log>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.LogType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Summary).IsRequired();
            });
        }
    }
}
