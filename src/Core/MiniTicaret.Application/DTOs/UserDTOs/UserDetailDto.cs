using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.DTOs.UserDTOs;

public class UserDetailDto
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }

    public DateTime? CreatedAt { get; set; }
    public int ProductCount { get; set; }
    public int OrderCount { get; set; }
    public int ReviewCount { get; set; }
    public int FavoriteCount { get; set; }
}
