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

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private readonly MiniTicaretDbContext _context;

    public CategoryRepository(MiniTicaretDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllWithSubCategoriesAsync()
    {
        return await _context.Categories
            .Include(c => c.SubCategories)
            .ToListAsync();
    }
}
