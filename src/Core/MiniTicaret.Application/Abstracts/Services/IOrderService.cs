using MiniTicaret.Application.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Abstracts.Services;

public interface IOrderService
{
    Task<Guid> CreateOrderAsync(OrderCreateDto dto, string buyerId);
    Task<List<OrderListDto>> GetMyOrdersAsync(string buyerId);
    Task<List<OrderListDto>> GetMySalesAsync(string sellerId);
    Task<OrderDetailDto> GetOrderByIdAsync(Guid orderId, string userId);
}
