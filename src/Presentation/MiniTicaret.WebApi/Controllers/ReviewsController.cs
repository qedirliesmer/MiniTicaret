using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniTicaret.Application.Abstracts.Services;
using MiniTicaret.Application.DTOs.ReviewDTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MiniTicaret.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddReview(Guid productId, [FromBody] ReviewCreateDto dto)
        {
            await _reviewService.AddReviewAsync(productId, dto, User);
            return Ok(new { Message = "Review added successfully" });
        }

        [HttpGet]
        public async Task<IActionResult> GetReviews(Guid productId)
        {
            var reviews = await _reviewService.GetReviewsByProductIdAsync(productId);
            return Ok(reviews);
        }

        [HttpPut("{reviewId}")]
        [Authorize]
        public async Task<IActionResult> UpdateReview(Guid productId, Guid reviewId, [FromBody] ReviewUpdateDto dto)
        {
            await _reviewService.UpdateReviewAsync(reviewId, dto, User);
            return Ok(new { Message = "Review updated successfully" });
        }

        [HttpDelete("{reviewId}")]
        [Authorize]
        public async Task<IActionResult> DeleteReview(Guid productId, Guid reviewId)
        {
            await _reviewService.DeleteReviewAsync(reviewId, User);
            return Ok(new { Message = "Review deleted successfully" });
        }
    }
}
