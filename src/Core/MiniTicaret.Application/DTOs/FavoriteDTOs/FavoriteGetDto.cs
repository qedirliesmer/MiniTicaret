using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.DTOs.FavoriteDTOs;

public class FavoriteGetDto
{
    public string ProductName { get; set; } = null!;
    public decimal Price { get; set; }
    public string? ProductImageUrl { get; set; }
    public Guid ProductId { get; set; }
    public string ProductTitle { get; set; }
}
