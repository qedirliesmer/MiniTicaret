using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.DTOs.ProductDTOs;

public class ProductDetailDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string CategoryName { get; set; }
    public string Description { get; set; }
    public List<string> ImageUrls { get; set; }
    public string OwnerFullName { get; set; }
}
