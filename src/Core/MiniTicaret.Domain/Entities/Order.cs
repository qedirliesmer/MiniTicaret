using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Domain.Entities;

public class Order:BaseEntity
{
    public string BuyerId { get; set; }
    public AppUser Buyer { get; set; } = null!;

    public DateTime OrderDate { get; set; }
    public string Status { get; set; }

    public ICollection<Order_Product> OrderProducts { get; set; }
}
