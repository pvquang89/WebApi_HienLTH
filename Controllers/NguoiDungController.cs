using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApi_HienLTH.Data;
using WebApi_HienLTH.Models;
using WebApi_HienLTH.Models.ModelsForJwt;

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
        public async Task<IActionResult> Validate(LoginModel model)
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
            //
            var token = await GenerateToken(user);
            //nếu đúng, cấp token
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Authenticate success",
                Data = token
            });

        }

        //return TokenModle
        private async Task<TokenModel> GenerateToken(NguoiDungEntity nguoiDung)
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
                    new Claim(JwtRegisteredClaimNames.Email,nguoiDung.Email),
                    new Claim(JwtRegisteredClaimNames.Sub,nguoiDung.Email),
                    ///?????
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim("UserName",nguoiDung.UserName),
                    new Claim("Id",nguoiDung.Id.ToString()),
                    //roles

                }),
                Expires = DateTime.UtcNow.AddSeconds(20),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey
                    (secretKeyByte), SecurityAlgorithms.HmacSha256)
            };
            //dựa trên thông tin cấu hình để tạo token
            var token = jwtTokenHandler.CreateToken(tokenDescription);

            //chuyển token thành chuỗi string để gửi về client
            var accessToken = jwtTokenHandler.WriteToken(token);
            //
            var refreshToken = GenerateRefreshToken();
            //lưu vào database
            var refreshTokenEntity = new RefreshToken
            {
                JwtId = token.Id,
                UserId=nguoiDung.Id,
                Token = refreshToken,
                IsUsed = false,
                IsRevoked = false,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddHours(1)
            };
            await _context.AddAsync(refreshTokenEntity);
            await _context.SaveChangesAsync();
            //

            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            //
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                //tại sao lại 64
                return Convert.ToBase64String(random);
            }
        }

        [HttpPost("RenewToken")]
        public async Task<IActionResult> RenewToken(TokenModel model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            //lấy mã mảng byte
            var secretKeyByte = Encoding.UTF8.GetBytes(_appSetting.SecretKey);

            //
            var tokenValidateParam = new TokenValidationParameters
            {
                // Tự cấp token
                ValidateIssuer = false,
                ValidateAudience = false,

                // Ký vào token
                ValidateIssuerSigningKey = true,
                //thuật toán đối xứng
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyByte),

                ClockSkew = TimeSpan.Zero, //ko cho thời gian trễ, token hết hạn sẽ từ chối ngay. Mặc định 5 phút


                ValidateLifetime = false    //?????????? ko kiểm tra về token hết hạn
            };
            //kiểm tra token valid 
            try
            {
                //check 1 : format access token có valid
                var tokenInVerification = jwtTokenHandler.ValidateToken(model.AccessToken,
                                                                        tokenValidateParam,
                                                                        out var validatedToken);

                //check 2 : check thuật toán
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if (!result)//false
                        return Ok(new ApiResponse
                        {
                            Success = false,
                            Message = "Invalid token"
                        });
                }
                //check 3 : check access token is expire ?
                var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
                if (expireDate > DateTime.UtcNow)
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Access token has not yet expired"
                    });

                //check 4 : check refreshtoken exist in db
                var storeToken = _context.RefreshTokens.FirstOrDefault(e => e.Token == model.RefreshToken);
                if (storeToken == null)
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token does not exsit "
                    });

                //check 5 : check refreshToken is used or revoked
                if (storeToken.IsUsed)
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token has been used "
                    });
                if (storeToken.IsRevoked)
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token has been revoked "
                    });

                //check 6 : Access token id == JwtId in refreshToken
                var jti = tokenInVerification.Claims.FirstOrDefault(e => e.Type == JwtRegisteredClaimNames.Jti).Value;
                if (storeToken.JwtId != jti)
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Token does not match "
                    });
                //update token is used
                storeToken.IsRevoked = true;
                storeToken.IsUsed = true;
                _context.Update(storeToken);
                await _context.SaveChangesAsync();

                //create new token
                var user = await _context.NguoiDungs.SingleOrDefaultAsync(e => e.Id == storeToken.UserId);
                var token = await GenerateToken(user);
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Renew token success",
                    Data = token
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

        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();

            return dateTimeInterval;
        }
    }
}
