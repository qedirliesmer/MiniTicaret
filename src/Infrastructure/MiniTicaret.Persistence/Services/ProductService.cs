using AutoMapper;
using MiniTicaret.Application.Abstracts.Repositories;
using MiniTicaret.Application.Abstracts.Services;
using MiniTicaret.Application.DTOs.ProductDTOs;
using MiniTicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Persistence.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<List<ProductGetDto>> GetAllAsync(Guid? categoryId, decimal? minPrice, decimal? maxPrice, string? search)
    {
        var products = await _productRepository.GetAllWithFilterAsync(categoryId, minPrice, maxPrice, search);
        return _mapper.Map<List<ProductGetDto>>(products);
    }

    public async Task<ProductDetailDto?> GetByIdAsync(Guid id)
    {
        var product = await _productRepository.GetByIdWithIncludesAsync(id);
        return _mapper.Map<ProductDetailDto>(product);
    }

    public async Task<ProductDetailDto> CreateAsync(ProductCreateDto dto, string userId)
    {
        var product = _mapper.Map<Product>(dto);
        product.OwnerId = userId;

        // Images əlavə et
        product.Images = dto.ImageUrls.Select(url => new Image { Image_Url = url }).ToList();

        await _productRepository.AddAsync(product);
        await _productRepository.SaveChangesAsync();

        var created = await _productRepository.GetByIdWithIncludesAsync(product.Id);
        return _mapper.Map<ProductDetailDto>(created);
    }

    public async Task<bool> UpdateAsync(Guid id, ProductUpdateDto dto, string userId)
    {
        var product = await _productRepository.GetByIdWithIncludesAsync(id);
        if (product == null || product.OwnerId != userId)
            return false;

        product.Title = dto.Title;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.CategoryId = dto.CategoryId;

        // Əvvəlki şəkilləri silib yeniləri əlavə edirik (simplified)
        product.Images.Clear();
        product.Images = dto.ImageUrls.Select(url => new Image { Image_Url = url }).ToList();

        _productRepository.Update(product);
        return await _productRepository.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(Guid id, string userId)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null || product.OwnerId != userId)
            return false;

        _productRepository.Delete(product);
        return await _productRepository.SaveChangesAsync();
    }

    public async Task<List<ProductGetDto>> GetMyProductsAsync(string userId)
    {
        var products = await _productRepository.GetByOwnerIdAsync(userId);
        return _mapper.Map<List<ProductGetDto>>(products);
    }
}
