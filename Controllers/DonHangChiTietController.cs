using Microsoft.AspNetCore.Mvc;
using WebApi_HienLTH.Models;
using WebApi_HienLTH.Repository;

namespace WebApi_HienLTH.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DonHangChiTietController : Controller
    {
        private readonly DonHangChiTietRepository _donHangChiTietRepository;
        public DonHangChiTietController(DonHangChiTietRepository donHangChiTietRepository)
        {
            _donHangChiTietRepository = donHangChiTietRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonHangChiTietModel>>> GetAll()
        {
            var entities = await _donHangChiTietRepository.GetAllAsync();
            return Ok(entities);
        }

        [HttpGet("{maDh}/{maHh}")]
        public async Task<ActionResult<DonHangChiTietModel>> GetById(int maDh, int maHh)
        {
            if (!await _donHangChiTietRepository.ExistsAsync(maDh, maHh))
                return NotFound("Đơn hàng chi tiết không tồn tại");
            var entities = await _donHangChiTietRepository.GetByIdAsync(maDh, maHh);
            return Ok(entities);
        }
        [HttpPost]
        public async Task<ActionResult<DonHangChiTietModel>> Create(DonHangChiTietModel model)
        {
            if (model == null)
                return BadRequest("Dữ liệu không hợp lệ.");

            await _donHangChiTietRepository.CreateAsync(model);
            return Ok("Thêm chi tiết đơn hàng thành công.");
        }
        [HttpPut("{maDh}/{maHh}")]
        public async Task<IActionResult> Update(int maDh, int maHh, DonHangChiTietModel model)
        {
            if (maDh != model.MaDh || maHh != model.MaHh)
                return BadRequest("Mã đơn hàng và mã hàng hóa không khớp.");

            await _donHangChiTietRepository.UpdateAsync(model);
            return NoContent();
        }

        [HttpDelete("{maDh}/{maHh}")]
        public async Task<IActionResult> Delete(int maDh, int maHh)
        {
            if (!await _donHangChiTietRepository.ExistsAsync(maDh, maHh))
                return NotFound("Đơn hàng chi tiết không tồn tại");

            await _donHangChiTietRepository.DeleteAsync(maDh, maHh);
            return NoContent();
        }
    }
}
