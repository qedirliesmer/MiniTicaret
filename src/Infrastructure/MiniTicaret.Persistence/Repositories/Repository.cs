using Microsoft.EntityFrameworkCore;
using MiniTicaret.Application.Abstracts.Repositories;
using MiniTicaret.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly MiniTicaretDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(MiniTicaretDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }
}
