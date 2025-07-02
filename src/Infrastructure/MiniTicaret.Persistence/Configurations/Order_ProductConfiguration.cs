using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniTicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Persistence.Configurations;

public class Order_ProductConfiguration : IEntityTypeConfiguration<Order_Product>
{
    public void Configure(EntityTypeBuilder<Order_Product> builder)
    {
        builder.HasKey(op => new { op.OrderId, op.ProductId });

        builder.HasOne(op => op.Order)
               .WithMany(o => o.OrderProducts)
               .HasForeignKey(op => op.OrderId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(op => op.Product)
               .WithMany(p => p.OrderProducts)
               .HasForeignKey(op => op.ProductId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Property(op => op.Quantity)
               .IsRequired();

        builder.Property(op => op.UnitPrice)
               .HasColumnType("decimal(18,2)")
               .IsRequired();
    }
}
