using MiniTicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Abstracts.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<List<Category>> GetAllWithSubCategoriesAsync();
    Task DeleteAsync(Category entity);
}
