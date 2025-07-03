using MiniTicaret.Application.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Abstracts.Services;

public interface IProductService
{
    Task<List<ProductGetDto>> GetAllAsync(Guid? categoryId, decimal? minPrice, decimal? maxPrice, string? search);
    Task<ProductDetailDto?> GetByIdAsync(Guid id);
    Task<ProductDetailDto> CreateAsync(ProductCreateDto dto, string userId);
    Task<bool> UpdateAsync(Guid id, ProductUpdateDto dto, string userId);
    Task<bool> DeleteAsync(Guid id, string userId);
    Task<List<ProductGetDto>> GetMyProductsAsync(string userId);
}
