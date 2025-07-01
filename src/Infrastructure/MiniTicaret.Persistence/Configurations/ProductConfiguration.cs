using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniTicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
               .IsRequired()
               .HasMaxLength(150);
     
        builder.Property(p => p.Description)
               .IsRequired();
     
        builder.Property(p => p.Price)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.HasOne(p => p.Category)
               .WithMany(c => c.Products)
               .HasForeignKey(p => p.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Owner)
               .WithMany(u => u.Products)
               .HasForeignKey(p => p.OwnerId)
               .OnDelete(DeleteBehavior.Restrict); 

        builder.HasMany(p => p.OrderProducts)
               .WithOne(op => op.Product)
               .HasForeignKey(op => op.ProductId);

        builder.HasMany(p => p.Reviews)
               .WithOne(r => r.Product)
               .HasForeignKey(r => r.ProductId);

        builder.HasMany(p => p.Favorites)
               .WithOne(f => f.Product)
               .HasForeignKey(f => f.ProductId);
    }
}
