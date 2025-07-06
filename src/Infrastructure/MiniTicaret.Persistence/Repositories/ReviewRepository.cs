using Microsoft.EntityFrameworkCore;
using MiniTicaret.Application.Abstracts.Repositories;
using MiniTicaret.Domain.Entities;
using MiniTicaret.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MiniTicaret.Persistence.Repositories;

public class ReviewRepository : Repository<Review>, IReviewRepository
{
    private readonly MiniTicaretDbContext _context;

    public ReviewRepository(MiniTicaretDbContext context) : base(context)
    {
        _context = context;
    }

    // Review-ə aid əlavə metodlar
    public async Task<bool> HasUserAlreadyReviewedAsync(string userId, Guid productId)
    {
        return await _context.Reviews
            .AnyAsync(r => r.UserId == userId && r.ProductId == productId);
    }

    public async Task<List<Review>> GetReviewsByProductIdAsync(Guid productId)
    {
        return await _context.Reviews
            .Include(r => r.User) // əgər User məlumatını da göstərmək istəyirsənsə
            .Where(r => r.ProductId == productId)
            .ToListAsync();
    }
    public async Task UpdateAsync(Review review)
    {
        _context.Reviews.Update(review);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Review review)
    {
        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();
    }
}
