using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Domain.Entities;

public class Review:BaseEntity
{
    public string UserId { get; set; }
    public AppUser User { get; set; } = null!;

    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int Rating { get; set; } 
    public string Comment { get; set; } = null!;
}
