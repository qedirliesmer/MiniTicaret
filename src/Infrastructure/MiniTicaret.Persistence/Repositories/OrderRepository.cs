using Microsoft.EntityFrameworkCore;
using MiniTicaret.Application.Abstracts.Repositories;
using MiniTicaret.Domain.Entities;
using MiniTicaret.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Persistence.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    private readonly MiniTicaretDbContext _context;

    public OrderRepository(MiniTicaretDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetOrdersByBuyerIdAsync(string buyerId)
    {
        return await _context.Orders
            .Where(o => o.BuyerId == buyerId)
            .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
            .ToListAsync();
    }

    public async Task<List<Order>> GetSalesBySellerIdAsync(string sellerId)
    {
        return await _context.OrderProducts
            .Where(op => op.Product.OwnerId == sellerId)
            .Select(op => op.Order)
            .Distinct()
            .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderDetailsAsync(Guid orderId)
    {
        return await _context.Orders
            .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
            .Include(o => o.Buyer)
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }

}
