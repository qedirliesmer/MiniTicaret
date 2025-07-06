using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MiniTicaret.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("private")]
        [Authorize]
        public IActionResult PrivateTest()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(new { Message = "Uğurla daxil oldun!", UserId = userId });
        }

        [HttpGet("public")]
        [AllowAnonymous]
        public IActionResult PublicTest()
        {
            return Ok("Bu endpointə token olmadan da daxil ola bilərsən.");
        }
    }
}
