using AutoMapper;
using MiniTicaret.Application.DTOs.ReviewDTOs;
using MiniTicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Mapping;

public class ReviewProfile:Profile
{
    public ReviewProfile()
    {
        CreateMap<Review, ReviewGetDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));
    }
}
