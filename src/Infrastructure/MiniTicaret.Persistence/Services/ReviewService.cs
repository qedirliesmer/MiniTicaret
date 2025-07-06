using AutoMapper;
using MiniTicaret.Application.Abstracts.Repositories;
using MiniTicaret.Application.Abstracts.Services;
using MiniTicaret.Application.DTOs.ReviewDTOs;
using MiniTicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Persistence.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public ReviewService(
        IReviewRepository reviewRepository,
        IUserRepository userRepository,
        IMapper mapper)
    {
        _reviewRepository = reviewRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task AddReviewAsync(Guid productId, ReviewCreateDto dto, ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            throw new Exception("User ID not found in token.");

        bool alreadyReviewed = await _reviewRepository.HasUserAlreadyReviewedAsync(userId, productId);
        if (alreadyReviewed)
            throw new Exception("You have already reviewed this product.");

        var review = _mapper.Map<Review>(dto);
        review.ProductId = productId;
        review.UserId = userId;
        review.CreatedAt = DateTime.UtcNow;

        await _reviewRepository.AddAsync(review);
    }

    public async Task<List<ReviewGetDto>> GetReviewsByProductIdAsync(Guid productId)
    {
        var reviews = await _reviewRepository.GetReviewsByProductIdAsync(productId);
        return _mapper.Map<List<ReviewGetDto>>(reviews);
    }

    public async Task DeleteReviewAsync(Guid reviewId, ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            throw new Exception("User ID not found in token.");

        var review = await _reviewRepository.GetByIdAsync(reviewId);
        if (review == null)
            throw new Exception("Review not found.");

        // İcazə yoxlaması - rəy sahibi yoxsa admin olmalıdır
        if (review.UserId != userId && !user.IsInRole("Admin"))
            throw new UnauthorizedAccessException("You do not have permission to delete this review.");

        await _reviewRepository.DeleteAsync(review);
    }

    public async Task UpdateReviewAsync(Guid reviewId, ReviewUpdateDto dto, ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            throw new Exception("User ID not found in token.");

        var review = await _reviewRepository.GetByIdAsync(reviewId);
        if (review == null)
            throw new Exception("Review not found.");

        // İcazə yoxlaması
        if (review.UserId != userId && !user.IsInRole("Admin"))
            throw new UnauthorizedAccessException("You do not have permission to update this review.");

        _mapper.Map(dto, review);
        review.UpdatedAt = DateTime.UtcNow;

        await _reviewRepository.UpdateAsync(review);
    }
}
