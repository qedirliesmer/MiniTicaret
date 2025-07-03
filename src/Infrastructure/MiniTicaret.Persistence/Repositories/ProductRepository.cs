using Microsoft.EntityFrameworkCore;
using MiniTicaret.Application.Abstracts.Repositories;
using MiniTicaret.Domain.Entities;
using MiniTicaret.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Persistence.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    private readonly MiniTicaretDbContext _context;

    public ProductRepository(MiniTicaretDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdWithIncludesAsync(Guid id)
    {
        return await _context.Products
            .Include(p => p.Images)
            .Include(p => p.Category)
            .Include(p => p.Owner)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Product>> GetAllWithFilterAsync(Guid? categoryId, decimal? minPrice, decimal? maxPrice, string? search)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .AsQueryable();

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        if (minPrice.HasValue)
            query = query.Where(p => p.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice.Value);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(p => p.Title.ToLower().Contains(search.ToLower()));

        return await query.ToListAsync();
    }

    public async Task<List<Product>> GetByOwnerIdAsync(string userId)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Where(p => p.OwnerId == userId)
            .ToListAsync();
    }
}
