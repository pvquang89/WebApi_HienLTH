using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi_HienLTH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NguoiDungController : ControllerBase
    {

        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(new { message = "Bạn đã được xác thực!" });
        }
    }
}
