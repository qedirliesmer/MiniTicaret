using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.DTOs.OrderDTOs;

public class OrderDetailDto
{
    public Guid Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; }
    public string BuyerFullName { get; set; }

    public List<OrderProductDetailDto> Products { get; set; } = new List<OrderProductDetailDto>();
}
public class OrderProductDetailDto
{
    public Guid ProductId { get; set; }
    public string Title { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
}
