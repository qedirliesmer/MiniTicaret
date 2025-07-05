using AutoMapper;
using MiniTicaret.Application.DTOs.UserDTOs;
using MiniTicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Mapping;

public class UserProfile:Profile
{
    public UserProfile()
    {
        CreateMap<AppUser, UserDetailDto>()
            .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count))
            .ForMember(dest => dest.OrderCount, opt => opt.MapFrom(src => src.Orders.Count))
            .ForMember(dest => dest.ReviewCount, opt => opt.MapFrom(src => src.Reviews.Count))
            .ForMember(dest => dest.FavoriteCount, opt => opt.MapFrom(src => src.Favorites.Count));

        CreateMap<AppUser, UserGetDto>()
            .ForMember(dest => dest.Roles, opt => opt.Ignore());
    }
}
