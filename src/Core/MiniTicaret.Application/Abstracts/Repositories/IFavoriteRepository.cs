using MiniTicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Abstracts.Repositories;

public interface IFavoriteRepository:IRepository<Favorite>
{
    Task<bool> IsFavoriteExistsAsync(string userId, Guid productId);
    Task<Favorite?> GetByUserAndProductAsync(string userId, Guid productId);
    Task<List<Favorite>> GetUserFavoritesAsync(string userId);
    Task DeleteAsync(Favorite favorite);
}
