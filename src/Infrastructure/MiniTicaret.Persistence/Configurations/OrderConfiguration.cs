using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniTicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.HasOne(o => o.Buyer)
               .WithMany(u => u.Orders)
               .HasForeignKey(o => o.BuyerId)
               .OnDelete(DeleteBehavior.Restrict); 

        builder.Property(o => o.OrderDate)
               .IsRequired();

        builder.Property(o => o.Status)
               .IsRequired()
               .HasMaxLength(50);

        builder.HasMany(o => o.OrderProducts)
               .WithOne(op => op.Order)
               .HasForeignKey(op => op.OrderId);
    }
}
