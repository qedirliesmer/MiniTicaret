using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Domain.Entities;

public class AppUser : IdentityUser
{
    public string FullName { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshExpireDate { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public ICollection<Product> Products { get; set; }
    public ICollection<Order> Orders { get; set; }
    public ICollection<Review> Reviews { get; set; }
    public ICollection<Favorite> Favorites { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

}
  
