using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Pizza.Models;

public partial class PizzaDbkozlovtsevContext : DbContext
{
    public PizzaDbkozlovtsevContext()
    {
    }

    public PizzaDbkozlovtsevContext(DbContextOptions<PizzaDbkozlovtsevContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<OrderItemOption> OrderItemOptions { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductOption> ProductOptions { get; set; }

    public virtual DbSet<ProductSize> ProductSizes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=192.168.147.54;Database=PizzaDBKozlovtsev;User Id=is;Password=1;TrustServerCertificate=true;");
    //=> optionsBuilder.UseSqlServer("Server=localhost;Database=PizzaDB;Trusted_Connection=true;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(100);
            entity.Property(e => e.State).HasMaxLength(2);
            entity.Property(e => e.Street).HasMaxLength(100);
            entity.Property(e => e.Zip).HasMaxLength(10);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");

            entity.Property(e => e.DeliveryCharge).HasColumnType("smallmoney");
            entity.Property(e => e.DeliveryCity).HasMaxLength(100);
            entity.Property(e => e.DeliveryState).HasMaxLength(2);
            entity.Property(e => e.DeliveryStreet).HasMaxLength(100);
            entity.Property(e => e.DeliveryZip).HasMaxLength(10);
            entity.Property(e => e.ItemsTotal).HasColumnType("smallmoney");
            entity.Property(e => e.Phone).HasMaxLength(100);

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Customer");

            entity.HasOne(d => d.OrderStatus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_OrderStatus");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("OrderItem");

            entity.Property(e => e.Instructions).HasMaxLength(255);
            entity.Property(e => e.TotalPrice).HasColumnType("smallmoney");
            entity.Property(e => e.UnitPrice).HasColumnType("smallmoney");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItem_Order");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItem_Product");

            entity.HasOne(d => d.ProductSize).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductSizeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItem_ProductSize");
        });

        modelBuilder.Entity<OrderItemOption>(entity =>
        {
            entity.ToTable("OrderItemOption");

            entity.Property(e => e.Price).HasColumnType("smallmoney");

            entity.HasOne(d => d.OrderItem).WithMany(p => p.OrderItemOptions)
                .HasForeignKey(d => d.OrderItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItemOption_OrderItem");

            entity.HasOne(d => d.ProductOption).WithMany(p => p.OrderItemOptions)
                .HasForeignKey(d => d.ProductOptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItemOption_ProductOption");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.ToTable("OrderStatus");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.HasOptions).HasDefaultValue(true);
            entity.Property(e => e.Image).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.SizeIds).HasMaxLength(10);
            entity.Property(e => e.Type).HasMaxLength(20);
        });

        modelBuilder.Entity<ProductOption>(entity =>
        {
            entity.ToTable("ProductOption");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.IsPizzaOption).HasDefaultValue(true);
            entity.Property(e => e.IsSaladOption).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(20);
        });

        modelBuilder.Entity<ProductSize>(entity =>
        {
            entity.ToTable("ProductSize");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.IsGlutenFree).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PremiumPrice).HasColumnType("smallmoney");
            entity.Property(e => e.Price).HasColumnType("smallmoney");
            entity.Property(e => e.ToppingPrice).HasColumnType("smallmoney");
            entity.Property(e => e.Type).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
