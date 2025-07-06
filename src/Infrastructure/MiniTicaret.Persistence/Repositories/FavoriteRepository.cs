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

public class FavoriteRepository : Repository<Favorite>, IFavoriteRepository
{
    private readonly MiniTicaretDbContext _context;

    public FavoriteRepository(MiniTicaretDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> IsFavoriteExistsAsync(string userId, Guid productId)
    {
        return await _context.Favorites
            .AnyAsync(f => f.UserId == userId && f.ProductId == productId);
    }

    public async Task<Favorite?> GetByUserAndProductAsync(string userId, Guid productId)
    {
        return await _context.Favorites
            .Include(f => f.Product)
            .ThenInclude(p => p.Images)
            .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);
    }


    public async Task<List<Favorite>> GetUserFavoritesAsync(string userId)
    {
        return await _context.Favorites
         .Include(f => f.Product)  // Product-u yüklə ki, Product.Title-ə çıxış olsun
         .Where(f => f.UserId == userId)
         .ToListAsync();
    }

    public async Task DeleteAsync(Favorite favorite)
    {
        _context.Favorites.Remove(favorite);
        await _context.SaveChangesAsync();
    }
}
