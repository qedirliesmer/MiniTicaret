using MiniTicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Abstracts.Repositories;

public interface IOrderRepository:IRepository<Order>
{
    Task<List<Order>> GetOrdersByBuyerIdAsync(string buyerId);
    Task<List<Order>> GetSalesBySellerIdAsync(string sellerId);
    Task<Order?> GetOrderDetailsAsync(Guid orderId);
}
