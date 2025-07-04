using MiniTicaret.Application.DTOs.CategoryDTOs;
using MiniTicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Abstracts.Services;

public interface ICategoryService
{
    Task<List<CategoryGetDto>> GetAllAsync();
    Task CreateAsync(CategoryCreateDto dto);
    Task UpdateAsync(Guid id, CategoryUpdateDto dto);
}
