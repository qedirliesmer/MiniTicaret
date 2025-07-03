using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Abstracts.Repositories;

public interface IRepository<T> where T : class
{
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<bool> SaveChangesAsync();
}
