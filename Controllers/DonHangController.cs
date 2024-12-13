using Microsoft.AspNetCore.Mvc;
using WebApi_HienLTH.Models;
using WebApi_HienLTH.Repository.Repository;
using WebApi_HienLTH.UnitOfWork;

namespace WebApi_HienLTH.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DonHangController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DonHangController(IUnitOfWork donHangRepository)
        {
            _unitOfWork = donHangRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonHangModel>>> GetAll()
        {
            var entities = await _unitOfWork.DonHangRepository.GetAllAsync();
            return Ok(entities);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DonHangModel>> GetById(int id)
        {
            var entity = await _unitOfWork.DonHangRepository.GetByIdAsync(id);
            return entity == null ? NotFound("Không tìm thấy") : Ok(entity);
        }

        [HttpPost]
        public async Task<ActionResult<DonHangModel>> Create(DonHangModel model)
        {
            await _unitOfWork.DonHangRepository.CreateAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = model.MaDh }, model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DonHangModel model)
        {
            if (id != model.MaDh)
                return BadRequest("Mã đơn hàng không khớp");
            if (!await _unitOfWork.DonHangRepository.ExistsAsync(id))
                return NotFound("Không tồn tại đơn hàng có id này");

            await _unitOfWork.DonHangRepository.UpdateAsync(model);
            return Ok("Cập nhật thành công");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _unitOfWork.DonHangRepository.ExistsAsync(id))
                return NotFound("Không thấy đơn hàng với id này");
            await _unitOfWork.DonHangRepository.DeleteAsync(id);
            return Ok("Xoá thành công");
        }
    }
}
