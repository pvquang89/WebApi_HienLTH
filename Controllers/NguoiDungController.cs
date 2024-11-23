using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Text;
using WebApi_HienLTH.Data;
using WebApi_HienLTH.Models;

namespace WebApi_HienLTH.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NguoiDungController : ControllerBase
    {

        private readonly MyDbContext _context;
        private readonly AppSetting _appSetting;

        //có thể lấy appsetting thông qua IConfiguration
        //ở đây dùng IOptionsMonitor
        public NguoiDungController(MyDbContext context, IOptionsMonitor<AppSetting> optionsMonitor)
        {
            _context = context;
            _appSetting = optionsMonitor.CurrentValue;
        }


        [HttpPost("Login")]
        public IActionResult Validate(LoginModel model)
        {
            var user = _context.NguoiDungs.SingleOrDefault(e => e.UserName == model.UserName
                                                            && e.Password == model.Password);
            //nếu user ko đúng
            if (user == null)
            {
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Invalid user name or password"
                });
            }
            //nếu đúng, cấp token
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Authenticate success",
                Data = GenerateToken(user)
            });

        }

        private string GenerateToken(NguoiDungEntity nguoiDung)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            //lấy mã mảng byte
            var secretKeyByte = Encoding.UTF8.GetBytes(_appSetting.SecretKey);
            //thiết lập cấu hình cho jwt token cần tạo
            var tokenDescription = new SecurityTokenDescriptor
            {
                //Cung cấp thông tin payload (các claims) bên trong JWT
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name,nguoiDung.HoTen),
                    new Claim(ClaimTypes.Email,nguoiDung.Email),
                    new Claim("UserName",nguoiDung.UserName),
                    new Claim("Id",nguoiDung.Id.ToString()),
                    new Claim("TokenId", Guid.NewGuid().ToString()),

                    //roles

                }),
                Expires = DateTime.UtcNow.AddMinutes(1),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey
                    (secretKeyByte), SecurityAlgorithms.HmacSha256)
            };
            //dựa trên thông tin cấu hình để tạo token
            var token = jwtTokenHandler.CreateToken(tokenDescription);
            //chuyển token thành chuỗi string để gửi về client
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
