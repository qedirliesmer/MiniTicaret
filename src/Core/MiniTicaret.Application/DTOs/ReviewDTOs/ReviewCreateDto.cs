using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.DTOs.ReviewDTOs;

public class ReviewCreateDto
{
    public int Rating { get; set; }
    public string Comment { get; set; } = null!;
}
