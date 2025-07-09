using AutoMapper;
using MiniTicaret.Application.DTOs.AuthenticationDTOs;
using MiniTicaret.Application.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Mapping;

public class MappingProfile:Profile
{
    public MappingProfile()
    {
        CreateMap<TokenResponse, AuthenticationTokenResponseDto>()
            .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.Token))
            .ForMember(dest => dest.ExpireDate, opt => opt.MapFrom(src => src.ExpireDate ?? DateTime.UtcNow.AddHours(2)));
    }
}
