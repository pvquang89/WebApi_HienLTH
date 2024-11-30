using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi_HienLTH.Data;
using WebApi_HienLTH.Models;
using WebApi_HienLTH.Models.JwtModel;
using WebApi_HienLTH.Models.ModelsForJwt;
using WebApi_HienLTH.Repository.NguoiDungRepository;

namespace WebApi_HienLTH.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly INguoiDungRepository _nguoiDungRepository;
        private readonly JwtSettings _jwtSettings;

        public AuthController(INguoiDungRepository nguoiDungRepository, IOptionsMonitor<JwtSettings> optionsMonitor)
        {
            _nguoiDungRepository = nguoiDungRepository;
            _jwtSettings = optionsMonitor.CurrentValue;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginModel request)
        {
            var user = _nguoiDungRepository.Authenticate(request.UserName, request.Password);

            if (user == null)
                return Unauthorized(new ApiResponse
                {
                    Success = false,
                    Message = "Invalid username or password"
                });

            //Nếu đúng, tạo token
            var token = GenerateJwtToken(user);

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Authenticate success",
                Data = token
            });
        }

        private string GenerateJwtToken(NguoiDungEntity user)
        {
            //chuyển secret key sang byte và mã hoá để cấp cho client
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            //xác định thuật toán và mã hoá 
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //claims : dữ liệu chứa trong token để gửi về client
            var claims = new[]
            {
                //sub : subject(chủ thể) thường là tên user
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                //Jti - jwt id : id của token 
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                //claims tuỳ chỉnh lưu id user
                new Claim("id", user.Id.ToString()),
                new Claim("HoTen", user.HoTen)
            };
            //tạo token 
            //JwtSecurityToken - đại diện cho 1 jwt token
            var token = new JwtSecurityToken(
                //issuer: _jwtSettings.Issuer,
                //audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(60),
                //cung cấp thông tin về cách ký : thuật toán, secret key
                signingCredentials: creds
            );
            //chuyển token từ JwtSecurityToken sang string 
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
