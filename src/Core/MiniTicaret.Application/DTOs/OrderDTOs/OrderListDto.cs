using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.DTOs.OrderDTOs;

public class OrderListDto
{
    public Guid Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; }
    public int ProductCount { get; set; }  
    public decimal TotalPrice { get; set; }
}
