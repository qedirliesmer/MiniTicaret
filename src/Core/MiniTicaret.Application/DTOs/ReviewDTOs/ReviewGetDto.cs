using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.DTOs.ReviewDTOs;

public class ReviewGetDto
{
    public Guid Id { get; set; }             
    public string UserName { get; set; } = null!; 
    public int Rating { get; set; }          
    public string Comment { get; set; } = null!; 
    public DateTime CreatedAt { get; set; }
}
