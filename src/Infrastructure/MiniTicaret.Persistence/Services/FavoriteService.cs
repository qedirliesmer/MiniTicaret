using MiniTicaret.Application.Abstracts.Repositories;
using MiniTicaret.Application.Abstracts.Services;
using MiniTicaret.Application.DTOs.FavoriteDTOs;
using MiniTicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Persistence.Services;
public class FavoriteService : IFavoriteService
{
    private readonly IFavoriteRepository _favoriteRepository;
    private readonly IUserRepository _userRepository;

    public FavoriteService(IFavoriteRepository favoriteRepository, IUserRepository userRepository)
    {
        _favoriteRepository = favoriteRepository;
        _userRepository = userRepository;
    }

    public async Task AddToFavoritesAsync(Guid productId, ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) throw new Exception("İstifadəçi tapılmadı");

        bool exists = await _favoriteRepository.IsFavoriteExistsAsync(userId, productId);
        if (exists) throw new Exception("Məhsul artıq favoritlərdədir.");

        var favorite = new Favorite
        {
            ProductId = productId,
            UserId = userId
        };

        await _favoriteRepository.AddAsync(favorite);
    }

    public async Task RemoveFromFavoritesAsync(Guid productId, ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) throw new Exception("İstifadəçi tapılmadı");

        var favorite = await _favoriteRepository.GetByUserAndProductAsync(userId, productId);
        if (favorite == null) throw new Exception("Favorit məhsul tapılmadı");

        await _favoriteRepository.DeleteAsync(favorite);
    }

    public async Task<List<FavoriteGetDto>> GetUserFavoritesAsync(ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) throw new Exception("İstifadəçi tapılmadı");

        var favorites = await _favoriteRepository.GetUserFavoritesAsync(userId);

        // Burada AutoMapper varsa, onu istifadə etmək yaxşıdır, yoxdursa, manual mapping edin:
        var result = favorites.Select(f => new FavoriteGetDto
        {
            ProductId = f.ProductId,
            ProductTitle = f.Product.Title,
            ProductImageUrl = f.Product.Images.FirstOrDefault()?.Image_Url ?? "", // ilk şəkil URL
            // istəsən digər lazımlı sahələr
        }).ToList();

        return result;
    }
}
