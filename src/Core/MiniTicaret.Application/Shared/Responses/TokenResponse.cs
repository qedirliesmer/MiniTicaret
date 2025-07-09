using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Shared.Responses;

public class TokenResponse
{
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? ExpireDate { get; set; }
}
