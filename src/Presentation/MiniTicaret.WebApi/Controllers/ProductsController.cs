using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniTicaret.Application.Abstracts.Services;
using MiniTicaret.Application.DTOs.ProductDTOs;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MiniTicaret.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // ✅ GET /api/products?categoryId=&minPrice=&maxPrice=&search=
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Guid? categoryId, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice, [FromQuery] string? search)
        {
            var result = await _productService.GetAllAsync(categoryId, minPrice, maxPrice, search);
            return Ok(result);
        }

        // ✅ GET /api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _productService.GetByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // ✅ POST /api/products
        [HttpPost]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _productService.CreateAsync(dto, userId!);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // ✅ PUT /api/products/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductUpdateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var success = await _productService.UpdateAsync(id, dto, userId!);

            if (!success)
                return Forbid();

            return NoContent();
        }

        // ✅ DELETE /api/products/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var success = await _productService.DeleteAsync(id, userId!);

            if (!success)
                return Forbid();

            return NoContent();
        }

        // ✅ GET /api/products/my
        [HttpGet("my")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> GetMyProducts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _productService.GetMyProductsAsync(userId!);
            return Ok(result);
        }
    }
}
