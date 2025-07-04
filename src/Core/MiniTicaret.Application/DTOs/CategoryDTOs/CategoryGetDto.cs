using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.DTOs.CategoryDTOs;

public class CategoryGetDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<CategoryGetDto> SubCategories { get; set; } = new();
}
