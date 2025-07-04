using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.DTOs.OrderDTOs;

public class OrderCreateDto
{
    public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
}
public class OrderItemDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
