using AutoMapper;
using MiniTicaret.Application.DTOs.FavoriteDTOs;
using MiniTicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Mapping;

public class FavoriteProfile:Profile
{
    public FavoriteProfile()
    {
        CreateMap<Favorite, FavoriteGetDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Title))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
            .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => src.Product.Images.FirstOrDefault() != null ? src.Product.Images.FirstOrDefault()!.Image_Url : null));
    }
}

