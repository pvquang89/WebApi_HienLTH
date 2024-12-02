using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
        private readonly MyDbContext _context;
        private readonly INguoiDungRepository _nguoiDungRepository;
        private readonly JwtSettings _jwtSettings;

        public AuthController(INguoiDungRepository nguoiDungRepository,
            IOptionsMonitor<JwtSettings> optionsMonitor, MyDbContext context)
        {
            _nguoiDungRepository = nguoiDungRepository;
            //optionsMonitor.CurrentValue trả về obj JwtSettings 
            _jwtSettings = optionsMonitor.CurrentValue;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel request)
        {
            var user = _nguoiDungRepository.Authenticate(request.UserName, request.Password);

            if (user == null)
                return Unauthorized(new ApiResponse
                {
                    Success = false,
                    Message = "Invalid username or password"
                });

            //Nếu đúng, tạo token
            var token = await GenerateToken(user);

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Authenticate success",
                Data = token
            });
        }

        [HttpPost]
        public async Task<IActionResult> RenewToken(TokenModel token)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            //ở đây kiểm tra format token và thuật toán token nên 
            var tokenValidateParam = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false, // Vì token đã hết hạn nên không kiểm tra thời hạn ở đây
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = secretKeyBytes,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                //check 1 : kiểm tra format token 
                var tokenInVerification = jwtTokenHandler.ValidateToken(token.AccessToken,
                                                                        tokenValidateParam,
                                                                        out var validatedToken);
                //check 2 : kiểm tra thuật toán 
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                                                                    //ko phân biệt hoa thường
                                                                    StringComparison.InvariantCultureIgnoreCase);

                    if (!result)
                    {
                        return BadRequest(new ApiResponse
                        {
                            Success = false,
                            Message = "Invalid token"
                        });
                    }
                }

                //check 3 : kiểm tra refresh token trong db ko 
                var storedRefreshToken = _context.RefreshTokens.FirstOrDefault(e => e.Token == token.RefreshToken);
                //có null ko 
                if (storedRefreshToken == null)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token does not exsit "
                    });
                }
                //đã dùng hay đã gỡ 
                if (storedRefreshToken.IsUsed || storedRefreshToken.IsRevoked)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token has already been used or revoked "
                    });
                }
                //còn hạn hay hết
                if (storedRefreshToken.ExpiredAt < DateTime.UtcNow)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token has expired "
                    });
                }
                //check 4 : Access token id == JwtId in refreshToken
                var jti = tokenInVerification.Claims.FirstOrDefault(e => e.Type == JwtRegisteredClaimNames.Jti).Value;
                if (storedRefreshToken.JwtId != jti)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Token does not match "
                    });
                }

                //update token is used
                storedRefreshToken.IsRevoked = true;
                storedRefreshToken.IsUsed = true;
                _context.Update(storedRefreshToken);
                await _context.SaveChangesAsync();

                //generate new tokens
                var userId = storedRefreshToken.UserId;
                var user = await _context.NguoiDungs.FirstOrDefaultAsync(e => e.Id == userId);
                if (user == null)
                    return NotFound(new { Message = "User not found" });

                //cấp bộ token mới
                var newToken = await GenerateToken(user); 
                //ở đây không cần lưu lại bộ token mới vào database
                return Ok(new ApiResponse
                {
                    Success=true,
                    Message = "Renew token success",
                    Data = newToken
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Something went wrong !"
                });
            }
        }

        //hàm này trả về 2 loại token
        private async Task<TokenModel> GenerateToken(NguoiDungEntity user)
        {
            //lấy access token 
            var accessToken = GenerateAccessToken(user, out string jwtId);
            //lấy refresh token
            var refreshToken = GenerateRefreshToken();
            //lưu refresh token vào database
            await SaveRefreshToken(jwtId, user.Id, refreshToken);

            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        //hàm tạo access token 
        private string GenerateAccessToken(NguoiDungEntity user, out string jwtId)
        {
            //chuyển secret key sang byte và mã hoá để cấp cho client
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            //xác định thuật toán và mã hoá 
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // Tạo jwtId
            jwtId = Guid.NewGuid().ToString();

            //claims : dữ liệu chứa trong token để gửi về client
            var claims = new[]
            {
                //sub : subject(chủ thể) thường là tên user
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                //Jti - jwt id : id của token 
                new Claim(JwtRegisteredClaimNames.Jti, jwtId),
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
                expires: DateTime.UtcNow.AddSeconds(20),
                //cung cấp thông tin về cách ký : thuật toán, secret key
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        //hàm save refresh token
        private async Task SaveRefreshToken(string jwtId, int userId, string refreshToken)
        {
            //lưu vào database
            var refreshTokenEntity = new RefreshToken
            {
                JwtId = jwtId,  //id của access token
                UserId = userId,
                Token = refreshToken,
                IsUsed = false,
                IsRevoked = false,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddHours(1)
            };
            await _context.AddAsync(refreshTokenEntity);
            await _context.SaveChangesAsync();
        }

        //hàm tạo refresh token 
        private string GenerateRefreshToken()
        {
            //tạo mảng chứa 32 kí tự
            var random = new byte[32];
            using (var rdg = RandomNumberGenerator.Create())
            {
                //điền giá trị ngẫu nhiên vào mảng
                rdg.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }


    }
}
