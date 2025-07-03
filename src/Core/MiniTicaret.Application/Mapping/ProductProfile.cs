using AutoMapper;
using MiniTicaret.Application.DTOs.ProductDTOs;
using MiniTicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Mapping;

public class ProductProfile:Profile
{
    public ProductProfile()
    {
        CreateMap<ProductCreateDto, Product>();
        CreateMap<ProductUpdateDto, Product>();

        CreateMap<Product, ProductGetDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

        CreateMap<Product, ProductDetailDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.OwnerFullName, opt => opt.MapFrom(src => src.Owner.FullName))
            .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.Images.Select(i => i.Image_Url)));
    }
}
