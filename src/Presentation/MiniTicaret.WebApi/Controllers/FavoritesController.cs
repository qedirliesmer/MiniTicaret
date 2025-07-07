using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniTicaret.Application.Abstracts.Services;
using MiniTicaret.Application.Shared.Permissions;
using System.Security.Claims;

namespace MiniTicaret.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;

        public FavoritesController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Product.AddProductFavourite)]
        public async Task<IActionResult> AddToFavorite(Guid productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            await _favoriteService.AddToFavoritesAsync(productId, User);
            return Ok(new { Message = "Product added to favorites." });
        }

        [HttpDelete]
        [Authorize(Policy = Permissions.Product.DeleteFavourite)]
        public async Task<IActionResult> RemoveFromFavorite(Guid productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            await _favoriteService.RemoveFromFavoritesAsync(productId, User);
            return Ok(new { Message = "Product removed from favorites." });
        }

        [HttpGet]
        [Route("~/api/favorite")]
        [Authorize(Policy = Permissions.Product.GetAllFavourite)]
        public async Task<IActionResult> GetUserFavorites()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var favorites = await _favoriteService.GetUserFavoritesAsync(User);
            return Ok(favorites);
        }
    }
}
