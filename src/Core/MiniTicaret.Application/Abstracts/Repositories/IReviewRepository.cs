using MiniTicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Abstracts.Repositories;

public interface IReviewRepository:IRepository<Review>
{
    Task<bool> HasUserAlreadyReviewedAsync(string userId, Guid productId);
    Task<List<Review>> GetReviewsByProductIdAsync(Guid productId);
    Task UpdateAsync(Review review);
    Task DeleteAsync(Review review);
}
