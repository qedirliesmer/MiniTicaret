using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.DTOs.CategoryDTOs;

public class CategoryCreateDto
{
    public string Name { get; set; }
    public Guid? ParentId { get; set; }
}
