using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Domain.Entities;

public class Image:BaseEntity
{
    public string Image_Url { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;

}
