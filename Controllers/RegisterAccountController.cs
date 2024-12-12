using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using WebApi_HienLTH.Data;
using WebApi_HienLTH.Models;
using WebApi_HienLTH.Models.MailModel;
using WebApi_HienLTH.Services.MailServices;

namespace WebApi_HienLTH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterAccountController : ControllerBase
    {

        private readonly MyDbContext _context;
        private readonly IEmailServices _emailRepository;

        public RegisterAccountController(MyDbContext context, IEmailServices emailRepository)
        {
            _context = context;
            _emailRepository = emailRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(NguoiDungModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Kiểm tra xem email hoặc username đã tồn tại chưa
            if (_context.NguoiDungs.Any(u => u.UserName == request.UserName || u.Email == request.Email))
            {
                return BadRequest("Username hoặc email đã tồn tại.");
            }

            // Tạo người dùng
            var nguoiDung = new NguoiDungEntity
            {
                UserName = request.UserName,
                Password = request.Password,
                Email = request.Email,
                HoTen = request.HoTen
            };
            _context.NguoiDungs.Add(nguoiDung);
            await _context.SaveChangesAsync();

            // Tạo mã xác nhận
            var confirmationCode = Guid.NewGuid().ToString();

            var token = new ConfirmationTokenEntity
            {
                UserId = nguoiDung.Id,
                Code = confirmationCode,
                Expiration = DateTime.UtcNow.AddHours(1) // Token hết hạn sau 1 giờ
            };
            _context.ConfirmationTokens.Add(token);
            await _context.SaveChangesAsync();

            // Gửi email xác nhận
            await _emailRepository.SendEmailAsync(request.Email, "Xác nhận đăng ký tài khoản",
                $"Mã xác nhận của bạn là: {confirmationCode}");

            return Ok("Đăng ký thành công. Vui lòng kiểm tra email để xác nhận tài khoản.");
        }


        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmAccount(string code)
        {
            var token = await _context.ConfirmationTokens.FirstOrDefaultAsync(t => t.Code == code);

            if (token == null || token.Expiration < DateTime.UtcNow)
            {
                return BadRequest("Mã xác nhận không hợp lệ hoặc đã hết hạn.");
            }

            var nguoiDung = await _context.NguoiDungs.FindAsync(token.UserId);
            if (nguoiDung == null)
            {
                return BadRequest("Người dùng không tồn tại.");
            }

            nguoiDung.IsActivated = true; // Thêm thuộc tính IsActivated trong bảng NguoiDungEntity
            _context.ConfirmationTokens.Remove(token);
            await _context.SaveChangesAsync();

            return Ok("Tài khoản đã được kích hoạt thành công.");
        }
    }
}
