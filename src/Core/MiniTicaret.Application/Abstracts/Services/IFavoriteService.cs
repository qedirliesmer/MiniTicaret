using MiniTicaret.Application.DTOs.FavoriteDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Abstracts.Services;

public interface IFavoriteService
{
    Task AddToFavoritesAsync(Guid productId, ClaimsPrincipal user);
    Task RemoveFromFavoritesAsync(Guid productId, ClaimsPrincipal user);
    Task<List<FavoriteGetDto>> GetUserFavoritesAsync(ClaimsPrincipal user);
}
