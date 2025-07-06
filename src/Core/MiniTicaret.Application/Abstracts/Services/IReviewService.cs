using MiniTicaret.Application.DTOs.ReviewDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Abstracts.Services;

public interface IReviewService
{
    /// <summary>
    /// Yeni rəy əlavə edir.
    /// </summary>
    Task AddReviewAsync(Guid productId, ReviewCreateDto dto, ClaimsPrincipal user);

    /// <summary>
    /// Verilmiş məhsula aid bütün rəyləri gətirir.
    /// </summary>
    Task<List<ReviewGetDto>> GetReviewsByProductIdAsync(Guid productId);
    Task DeleteReviewAsync(Guid reviewId, ClaimsPrincipal user);
    Task UpdateReviewAsync(Guid reviewId, ReviewUpdateDto dto, ClaimsPrincipal user);
}
