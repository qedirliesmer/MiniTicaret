using MiniTicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Abstracts.Repositories;

public interface IProductRepository:IRepository<Product>
{
    Task<Product?> GetByIdWithIncludesAsync(Guid id);
    Task<List<Product>> GetAllWithFilterAsync(Guid? categoryId, decimal? minPrice, decimal? maxPrice, string? search);
    Task<List<Product>> GetByOwnerIdAsync(string userId);
}
