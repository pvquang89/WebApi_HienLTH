using Microsoft.AspNetCore.Mvc;
using WebApi_HienLTH.Models;
using WebApi_HienLTH.Repository;
using WebApi_HienLTH.UnitOfWork;

namespace WebApi_HienLTH.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DonHangChiTietController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DonHangChiTietController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonHangChiTietModel>>> GetAll(string? tenHh, int? maDh, string? sortBy, int page = 1)
        {
            var entities = await _unitOfWork.DonHangChiTietRepository.GetAllAsync(tenHh, maDh, sortBy, page);
            if (!entities.Any())
            {
                return NotFound("Ko tìm thấy");
            }
            return Ok(entities);
        }

        [HttpGet("{maDh}/{maHh}")]
        public async Task<ActionResult<DonHangChiTietModel>> GetById(int maDh, int maHh)
        {
            if (!await _unitOfWork.DonHangChiTietRepository.ExistsAsync(maDh, maHh))
                return NotFound("Đơn hàng chi tiết không tồn tại");
            var entities = await _unitOfWork.DonHangChiTietRepository.GetByIdAsync(maDh, maHh);
            return Ok(entities);
        }
        [HttpPost]
        public async Task<ActionResult<DonHangChiTietModel>> Create(DonHangChiTietModel model)
        {
            if (model == null)
                return BadRequest("Dữ liệu không hợp lệ.");

            await _unitOfWork.DonHangChiTietRepository.CreateAsync(model);
            return Ok("Thêm chi tiết đơn hàng thành công.");
        }
        [HttpPut("{maDh}/{maHh}")]
        public async Task<IActionResult> Update(int maDh, int maHh, DonHangChiTietModel model)
        {
            if (maDh != model.MaDh || maHh != model.MaHh)
                return BadRequest("Mã đơn hàng và mã hàng hóa không khớp.");

            await _unitOfWork.DonHangChiTietRepository.UpdateAsync(model);
            return NoContent();
        }

        [HttpDelete("{maDh}/{maHh}")]
        public async Task<IActionResult> Delete(int maDh, int maHh)
        {
            if (!await _unitOfWork.DonHangChiTietRepository.ExistsAsync(maDh, maHh))
                return NotFound("Đơn hàng chi tiết không tồn tại");

            await _unitOfWork.DonHangChiTietRepository.DeleteAsync(maDh, maHh);
            return NoContent();
        }

        [HttpGet("{maDh}")]
        public async Task<ActionResult<double>> GetTotalValueByMaDonHang(int maDh)
        {
            if (!await _unitOfWork.DonHangChiTietRepository.ExistsAsync(maDh))
            {
                return NotFound();
            }
            var totalValue = await _unitOfWork.DonHangChiTietRepository.GetTotalValueByMaDh(maDh);
            return Ok(totalValue);
        }


    }
}
