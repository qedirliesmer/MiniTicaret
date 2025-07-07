using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniTicaret.Application.Abstracts.Services;
using MiniTicaret.Application.DTOs.OrderDTOs;
using MiniTicaret.Application.Shared.Permissions;
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
        [Authorize(Policy = Permissions.Order.Create)]
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
        [Authorize(Policy = Permissions.Order.GetMy)]
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
        [Authorize(Policy = Permissions.Order.GetMySales)]
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
        [Authorize(Policy = Permissions.Order.GetDetail)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orderDetail = await _orderService.GetOrderByIdAsync(id, userId!);
            return Ok(orderDetail);
        }
    }
}
