using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniTicaret.Application.Abstracts.Services;
using MiniTicaret.Application.DTOs.OrderDTOs;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MiniTicaret.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Sifariş yarat
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Buyer,Admin")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto dto)
        {
            var buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orderId = await _orderService.CreateOrderAsync(dto, buyerId!);
            return CreatedAtAction(nameof(GetById), new { id = orderId }, new { id = orderId });
        }

        /// <summary>
        /// Aktiv istifadəçinin etdiyi sifarişlər
        /// </summary>
        [HttpGet("my")]
        [Authorize(Roles = "Buyer,Admin")]
        public async Task<IActionResult> GetMyOrders()
        {
            var buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _orderService.GetMyOrdersAsync(buyerId!);
            return Ok(orders);
        }

        /// <summary>
        /// Aktiv istifadəçinin satdığı məhsullara gələn sifarişlər
        /// </summary>
        [HttpGet("my-sales")]
        [Authorize(Roles = "Seller,Admin")]
        public async Task<IActionResult> GetMySales()
        {
            var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var sales = await _orderService.GetMySalesAsync(sellerId!);
            return Ok(sales);
        }

        /// <summary>
        /// Sifarişin ətraflı məlumatı
        /// </summary>
        [HttpGet("{id}")]
        [Authorize] // Buyer və ya Seller daxil ola bilir
        public async Task<IActionResult> GetById(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orderDetail = await _orderService.GetOrderByIdAsync(id, userId!);
            return Ok(orderDetail);
        }
    }
}
