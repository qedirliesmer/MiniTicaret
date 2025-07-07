using AutoMapper;
using MiniTicaret.Application.Abstracts.Repositories;
using MiniTicaret.Application.Abstracts.Services;
using MiniTicaret.Application.DTOs.OrderDTOs;
using MiniTicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Persistence.Services;

public class OrderService : IOrderService
{
    private readonly IEmailService _emailService;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, IMapper mapper, IEmailService emailService)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _mapper = mapper;
        _emailService = emailService;
    }

    public async Task<Guid> CreateOrderAsync(OrderCreateDto dto, string buyerId)
    {
        var order = new Order
        {
            BuyerId = buyerId,
            OrderDate = DateTime.UtcNow,
            Status = "Pending",  // statusu istəyə görə dəyişə bilərsən
            OrderProducts = new List<Order_Product>()
        };

        foreach (var item in dto.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null)
                throw new Exception($"Product with id {item.ProductId} not found");

            order.OrderProducts.Add(new Order_Product
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            });
        }

        await _orderRepository.AddAsync(order);
        await _orderRepository.SaveChangesAsync();

        return order.Id;
    }

    public async Task<List<OrderListDto>> GetMyOrdersAsync(string buyerId)
    {
        var orders = await _orderRepository.GetOrdersByBuyerIdAsync(buyerId);
        return _mapper.Map<List<OrderListDto>>(orders);
    }

    public async Task<List<OrderListDto>> GetMySalesAsync(string sellerId)
    {
        var sales = await _orderRepository.GetSalesBySellerIdAsync(sellerId);
        return _mapper.Map<List<OrderListDto>>(sales);
    }

    public async Task<OrderDetailDto> GetOrderByIdAsync(Guid orderId, string userId)
    {
        var order = await _orderRepository.GetOrderDetailsAsync(orderId);
        if (order == null)
            throw new Exception("Order not found");

        // Yoxla ki, sorğunu edən user sifarişin sahibi (buyer) və ya məhsulların sahibi (seller) olsun
        var isBuyer = order.BuyerId == userId;
        var isSeller = order.OrderProducts.Any(op => op.Product.OwnerId == userId);

        if (!isBuyer && !isSeller)
            throw new UnauthorizedAccessException("You do not have access to this order");

        return _mapper.Map<OrderDetailDto>(order);
    }
}
