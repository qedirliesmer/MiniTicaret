using AutoMapper;
using MiniTicaret.Application.Abstracts.Repositories;
using MiniTicaret.Application.Abstracts.Services;
using MiniTicaret.Application.DTOs.CategoryDTOs;
using MiniTicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Persistence.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<CategoryGetDto>> GetAllAsync()
    {
        var categories = await _repository.GetAllWithSubCategoriesAsync();
        return _mapper.Map<List<CategoryGetDto>>(categories);
    }

    public async Task CreateAsync(CategoryCreateDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            ParentId = dto.ParentId
        };

        await _repository.AddAsync(category);
        await _repository.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, CategoryUpdateDto dto)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category == null)
            throw new Exception("Kateqoriya tapılmadı");

        category.Name = dto.Name;
        category.ParentId = dto.ParentId;

        await _repository.SaveChangesAsync();
    }
}
