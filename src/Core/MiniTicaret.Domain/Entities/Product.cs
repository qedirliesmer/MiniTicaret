using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Domain.Entities;

public class Product:BaseEntity
{
    public string Title{ get; set; }
    public string Description { get; set; }
    public decimal Price{ get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }

    public string OwnerId { get; set; }
    public AppUser Owner { get; set; }

    public ICollection<Order_Product> OrderProducts { get; set; }
    public ICollection<Review> Reviews { get; set; }
    public ICollection<Favorite> Favorites { get; set; }
    public ICollection<Image> Images { get; set; }
}
