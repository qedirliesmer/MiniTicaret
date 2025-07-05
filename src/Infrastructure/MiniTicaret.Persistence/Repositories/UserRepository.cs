using Microsoft.AspNetCore.Identity;
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

public class UserRepository : Repository<AppUser>, IUserRepository
{
    private readonly MiniTicaretDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public UserRepository(MiniTicaretDbContext context, UserManager<AppUser> userManager) : base(context)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<AppUser?> GetByIdWithRelationsAsync(string id)
    {
        return await _context.Users
            .Include(u => u.Products)
            .Include(u => u.Orders)
            .Include(u => u.Reviews)
            .Include(u => u.Favorites)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<List<AppUser>> GetAllUsersWithRolesAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<List<string>> GetUserRolesAsync(AppUser user)
    {
        return (await _userManager.GetRolesAsync(user)).ToList();
    }
    public async Task<AppUser?> GetByIdAsync(string id)
    {
        return await _context.Users.FindAsync(id);
    }
}
